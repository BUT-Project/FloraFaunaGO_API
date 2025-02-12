using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Dto.Response;
using FloraFauna_GO_Shared;
using System.Text.Json;

namespace FloraFauna_GO_Entities2Dto;

public class IdentificationService
{
    private IEspeceRepository<EspeceNormalDto, FullEspeceDto> Service { get; set; }
    private HttpClient client = new HttpClient();
    private MultipartFormDataContent form = new MultipartFormDataContent();

    private const string API_KEY = "2b10Pg3bHxg7lUNrD6FHVgxmu";
    private static readonly string apiEndpoint = $"https://my-api.plantnet.org/v2/identify/all?api-key={API_KEY}";

    public IdentificationService(IEspeceRepository<EspeceNormalDto, FullEspeceDto> service)
    {
        Service = service;
    }

    public async Task<FullEspeceDto> identify(AnimalIdentifyNormalDto dto)
    {
        var imageContent = new ByteArrayContent(dto.AskedImage);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

        // Create a new form for each request to avoid accumulating data
        using var form = new MultipartFormDataContent();
        form.Add(imageContent, "images", "image.jpg");

        HttpResponseMessage response = await client.PostAsync(apiEndpoint, form);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Réponse brute de l'API : " + jsonResponse);
            IdentificationResultDto dtoResult = JsonSerializer.Deserialize<IdentificationResultDto>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ResultDto resultDto = dtoResult.Results.OrderByDescending(r => r.Score).FirstOrDefault();
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
                    Image = dto.AskedImage,
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
}

