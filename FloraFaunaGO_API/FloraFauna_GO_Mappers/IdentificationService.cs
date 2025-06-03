using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Dto.Response;
using FloraFauna_GO_Shared;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace FloraFauna_GO_Entities2Dto;

public class IdentificationService
{
    private IEspeceRepository<EspeceNormalDto, FullEspeceDto> Service { get; set; }
    private HttpClient client = new HttpClient();
    private MultipartFormDataContent form = new MultipartFormDataContent();

    private const string PLANT_API_KEY = "2b10Pg3bHxg7lUNrD6FHVgxmu";
    private const string INSECT_API_KEY = "If5WLVV1F1DfPvA0d7gyjvf0MV6UH6RZ29dG0Wool6YxRRrgHW";
    private const string DATA_API_KEY = "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";
    private static readonly string plantApiEndpoint = $"https://my-api.plantnet.org/v2/identify/all?lang=fr&api-key={PLANT_API_KEY}";
    private static readonly string insectApiEndpoint = $"https://insect.kindwise.com/api/v1/identification?details=common_names,url,description,image";
    private static readonly string animalApiEndpoint = $"http://codefirst.iut.uca.fr/containers/FloraFauna_GO-identification-api/FloraFaunaGo_API/identification/animal";
    private static readonly string dataApiEndpoint = $"https://api.groq.com/openai/v1/chat/completions";

    private static readonly string filePath = "data_text_context.txt";
    public IdentificationService(IEspeceRepository<EspeceNormalDto, FullEspeceDto> service)
    {
        Service = service;
    }

    public async Task<FullEspeceDto> identify(AnimalIdentifyNormalDto dto, EspeceType type)
    {
        var imageContent = new ByteArrayContent(dto.AskedImage);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        string? speciesName = "";
        switch (type)
        {
            case EspeceType.Plant:
                form.Add(imageContent, "images", "image.jpg");
                speciesName = await IdentifyPlant(form, dto.AskedImage);
                break;
            case EspeceType.Animal:
                speciesName = await IdentifyAnimal(dto.AskedImage);
                break;
            case EspeceType.Insect:
                speciesName = await IdentifyInsect(dto.AskedImage);
                break;
            default:
                break;
        }
        var especes = Service.GetEspeceByName(speciesName);
        if (!speciesName.IsNullOrEmpty() && especes.Result is null)
        {
            var espece = await RetrieveFloraFaunaDatas(speciesName);
            espece.Espece.Nom = speciesName;
            return espece;
        }
        else return null;
    }

    private async Task<String> IdentifyPlant(MultipartFormDataContent form, byte[] image)
    {
        HttpResponseMessage response = await client.PostAsync(plantApiEndpoint, form);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Réponse brute de l'API : " + jsonResponse);
            PlantIdentificationResultDto dtoResult = JsonSerializer.Deserialize<PlantIdentificationResultDto>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            PlantResultDto resultDto = dtoResult.Results.OrderByDescending(r => r.Score).FirstOrDefault();
            return resultDto.Species.CommonNames?.FirstOrDefault();
        }
        return null;
    }

    private async Task<String> IdentifyInsect(byte[] image)
    {
        var base64Image = Convert.ToBase64String(image);

        var requestBody = new
        {
            images = new[] { base64Image }
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Api-Key", INSECT_API_KEY);

        HttpResponseMessage response = await client.PostAsync(insectApiEndpoint, jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Réponse brute de l'API : " + jsonResponse);

            var dtoResult = JsonSerializer.Deserialize<InsectIdentificationResultDto>(
                jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var bestSuggestion = dtoResult?.Result?.Classification?.Suggestions
                ?.OrderByDescending(s => s.Probability)
                ?.FirstOrDefault();

            if (bestSuggestion is null)
                return null;

            return bestSuggestion.Details?.CommonNames?.FirstOrDefault();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erreur {response.StatusCode} : {errorContent}");
        }

        return null;
    }

    public async Task<String> IdentifyAnimal(byte[] image)
    {
        var base64Image = Convert.ToBase64String(image);

        var requestBody = new
        {
            base64_image = base64Image
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        client.DefaultRequestHeaders.Clear();

        HttpResponseMessage response = await client.PostAsync(animalApiEndpoint, jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Réponse brute de l'API : " + jsonResponse);

            var dtoResult = JsonSerializer.Deserialize<AnimalIdentificationResultDto>(
                jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return dtoResult.Predictions.First().Prediction.Split(';').Last();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erreur {response.StatusCode} : {errorContent}");
        }

        return null;
    }

    public async Task<FullEspeceDto> RetrieveFloraFaunaDatas(string nom)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Fichier introuvable : {filePath}");
            return null;
        }

        var prompt = await File.ReadAllTextAsync(filePath, Encoding.UTF8);

        var requestBody = new
        {
            model = "meta-llama/llama-4-scout-17b-16e-instruct",
            messages = new[]
            {
            new { role = "user", content = prompt + nom }
        },
            temperature = 1,
            max_tokens = 1024,
            top_p = 1,
            stream = true,
            stop = (string?)null
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(dataApiEndpoint, jsonContent);

        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            var fullContent = new StringBuilder();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("data:"))
                {
                    var jsonLine = line.Substring(5).Trim();

                    if (jsonLine == "[DONE]")
                        break;

                    var jsonDoc = JsonDocument.Parse(jsonLine);
                    var delta = jsonDoc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("delta");

                    if (delta.TryGetProperty("content", out var content))
                    {
                        fullContent.Append(content.GetString());
                    }
                }
            }
            var fullText = fullContent.ToString();

            var dtoResult = JsonSerializer.Deserialize<FullEspeceDto>(fullText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dtoResult;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erreur {response.StatusCode}: {errorContent}");
        }

        return null;
    }
}

