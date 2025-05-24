using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Dto.Response;
using FloraFauna_GO_Shared;
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
    private static readonly string plantApiEndpoint = $"https://my-api.plantnet.org/v2/identify/all?lang=fr&api-key={PLANT_API_KEY}";
    private static readonly string insectApiEndpoint = $"https://insect.kindwise.com/api/v1/identification?details=common_names,url,description,image";

    public IdentificationService(IEspeceRepository<EspeceNormalDto, FullEspeceDto> service)
    {
        Service = service;
    }

    public async Task<FullEspeceDto> identify(AnimalIdentifyNormalDto dto, EspeceType type)
    {
        var imageContent = new ByteArrayContent(dto.AskedImage);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

        switch (type)
        {
            case EspeceType.Plant:
                form.Add(imageContent, "images", "image.jpg");
                return await IdentifyPlant(form, dto.AskedImage);
            case EspeceType.Animal:
                return null;
            case EspeceType.Insect:
                return await IdentifyInsect(dto.AskedImage);
            default:
                return null;
        }
    }

    private async Task<FullEspeceDto> IdentifyPlant(MultipartFormDataContent form, byte[] image)
    {
        HttpResponseMessage response = await client.PostAsync(plantApiEndpoint, form);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Réponse brute de l'API : " + jsonResponse);
            PlantIdentificationResultDto dtoResult = JsonSerializer.Deserialize<PlantIdentificationResultDto>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            PlantResultDto resultDto = dtoResult.Results.OrderByDescending(r => r.Score).FirstOrDefault();
            var allEspeces = await Service.GetAllEspece();
            var espece = allEspeces.Items.FirstOrDefault(e => e.Espece.Nom_Scientifique == resultDto.Species.ScientificName);

            if (espece != null)
                return espece;

            espece = new FullEspeceDto
            {
                Espece = new EspeceNormalDto
                {
                    Nom = resultDto.Species.CommonNames?.FirstOrDefault() ?? "Nom inconnu",
                    Nom_Scientifique = resultDto.Species.ScientificName,
                    Description = $"Espèce de la famille {resultDto.Species.Family?.ScientificNameWithoutAuthor ?? "inconnue"}.",
                    Image = image,
                    Image3D = null,
                    Famille = resultDto.Species.Family?.ScientificName ?? "Inconnue",
                    Zone = "À définir",
                    Climat = "À définir",
                    Regime = "À définir"
                },
                localisationNormalDtos = []
            };
            return espece;
        }
        return null;
    }

    private async Task<FullEspeceDto> IdentifyInsect(byte[] image)
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

            var allEspeces = await Service.GetAllEspece();
            var espece = allEspeces.Items.FirstOrDefault(e =>
                e.Espece.Nom_Scientifique == bestSuggestion.Name);

            if (espece is not null)
                return espece;

            espece = new FullEspeceDto
            {
                Espece = new EspeceNormalDto
                {
                    Nom = bestSuggestion.Details?.CommonNames?.FirstOrDefault() ?? "Nom inconnu",
                    Nom_Scientifique = bestSuggestion.Name,
                    Description = bestSuggestion.Details?.Description?.Value?.Trim()
                                  ?? "Description non disponible.",
                    Image = image,
                    Image3D = null,
                    Famille = bestSuggestion.Details?.EntityId ?? "Inconnue",
                    Zone = "À définir",
                    Climat = "À définir",
                    Regime = "À définir"
                },
                localisationNormalDtos = []
            };

            return espece;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erreur {response.StatusCode} : {errorContent}");
        }

        return null;
    }


}

