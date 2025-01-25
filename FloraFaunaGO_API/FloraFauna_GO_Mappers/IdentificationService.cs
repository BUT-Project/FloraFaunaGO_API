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
        using var memoryStream = new MemoryStream();
        await dto.AskedImage.CopyToAsync(memoryStream);
        var imageContent = new ByteArrayContent(memoryStream.ToArray());
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.AskedImage.ContentType);

        form.Add(imageContent, "images", dto.AskedImage.FileName);

        HttpResponseMessage response = await client.PostAsync(apiEndpoint, form);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            IdentificationResultDto dtoResult = JsonSerializer.Deserialize<IdentificationResultDto>(jsonResponse);
            var allEspeces = await Service.GetAllEspece();
            var espece = allEspeces.Items.FirstOrDefault(e => e.Espece.Nom_Scientifique == dtoResult.ScientificNameWithoutAuthor);
            if (espece != null)
                return espece;
            espece = new FullEspeceDto
            {
                Espece = new EspeceNormalDto
                {
                    Nom = dtoResult.ScientificNameWithoutAuthor,
                    Nom_Scientifique = dtoResult.ScientificNameWithoutAuthor,
                    Description = "Description",
                    Famille = dtoResult.Family.ScientificNameWithoutAuthor,
                    Zone = "Zone",
                    Climat = "Climat",
                    Regime = "Regime",
                    Image = memoryStream.ToArray(),
                },
                localisationNormalDtos = null
            };
            return espece;
        }
        return null;
    }
}

