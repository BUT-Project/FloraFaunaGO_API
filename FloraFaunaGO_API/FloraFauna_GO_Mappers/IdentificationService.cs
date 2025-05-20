using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Dto.Response;
using FloraFauna_GO_Shared;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace FloraFauna_GO_Entities2Dto;

public class IdentificationService
{
    private IEspeceRepository<EspeceNormalDto, FullEspeceDto> Service { get; set; }
    private HttpClient client = new HttpClient();
    private MultipartFormDataContent form = new MultipartFormDataContent();

    private const string PLANT_API_KEY = "2b10Pg3bHxg7lUNrD6FHVgxmu";
    private const string INSECT_API_KEY = "RYhPBNogMQ3V3v89QnnV4Qmyxh7KN5dKS3we9ljdtvPYdrY17u";
    private static readonly string plantApiEndpoint = $"https://my-api.plantnet.org/v2/identify/all?lang=fr&api-key={PLANT_API_KEY}";
    private static readonly string insectApiEndpoint = $"https://insect.kindwise.com/api/v1/identification";

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
                return await identifyPlant(form, dto.AskedImage);
            case EspeceType.Animal:
                return null;
            case EspeceType.Insect:
                return null;
            default:
                return null;
        }
    }

    private async Task<FullEspeceDto> identifyPlant(MultipartFormDataContent form, byte[] image)
    {
        HttpResponseMessage response = await client.PostAsync(plantApiEndpoint, form);
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

}

