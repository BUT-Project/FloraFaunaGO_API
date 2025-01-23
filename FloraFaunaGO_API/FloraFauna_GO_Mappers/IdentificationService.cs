using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Dto.Response;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities2Dto;

internal class IdentificationService
{
    private EspeceService Service { get; set; }
    private HttpClient client = new HttpClient();
    private MultipartFormDataContent form = new MultipartFormDataContent();

    private readonly string imagepath = "C:\\Users\\kyure\\Downloads\\Rose_Papa_Meilland.jpg";

    private const string API_KEY = "2b10Pg3bHxg7lUNrD6FHVgxmu";
    private static readonly string apiEndpoint = $"https://my-api.plantnet.org/v2/identify/all?api-key={API_KEY}";

    public IdentificationService(EspeceService service)
    {
        Service = service;
    }

    public async Task<FullEspeceDto> identify(AnimalIdentifyNormalDto dto)
    {
        var image = new StreamContent(File.OpenRead(imagepath));
        image.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

        form.Add(image, "images", Path.GetFileName(imagepath));

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
                    Image = image.ReadAsByteArrayAsync().Result,
                },
                localisationNormalDtos = null
            };
            return espece;
        }
        return null;
    }
}
