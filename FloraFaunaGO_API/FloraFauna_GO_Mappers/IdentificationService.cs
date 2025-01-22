using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities2Dto;

public class IdentificationService
{
    private IEspeceRepository<EspeceEntities> Repository { get; set; }
    private HttpClient client = new HttpClient();

    private const string API_KEY = "2b10Pg3bHxg7lUNrD6FHVgxmu";
    private static readonly string apiEndpoint = $"https://my-api.plantnet.org/v2/identify/all?api-key={API_KEY}";

    public IdentificationService(IEspeceRepository<EspeceEntities> repository)
    {
        Repository = repository;
    }

    public async Task<FullEspeceDto> identify(AnimalIdentifyNormalDto dto)
    {

    }
}

/*
    private const string API_KEY = "YOUR-PRIVATE-API-KEY-HERE"; // Remplacez par votre clé API
    private const string PROJECT = "all"; // Essayez des floras spécifiques : "weurope", "canada"…
    private static readonly string apiEndpoint = $"https://my-api.plantnet.org/v2/identify/{PROJECT}?api-key={API_KEY}";

    static async Task Main(string[] args)
    {
        string imagePath1 = @"../data/image_1.jpeg";
        string imagePath2 = @"../data/image_2.jpeg";

        // Créer un client HttpClient
        using (HttpClient client = new HttpClient())
        using (MultipartFormDataContent form = new MultipartFormDataContent())
        {
            // Ajouter les images
            form.Add(new StreamContent(File.OpenRead(imagePath1)), "images", Path.GetFileName(imagePath1));
            form.Add(new StreamContent(File.OpenRead(imagePath2)), "images", Path.GetFileName(imagePath2));

            // Ajouter les autres données (par exemple, organs)
            var data = new Dictionary<string, string> { { "organs", "flower,leaf" } };

            foreach (var item in data)
            {
                form.Add(new StringContent(item.Value), item.Key);
            }

            // Configurer la requête HTTP
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Envoyer la requête POST
            HttpResponseMessage response = await client.PostAsync(apiEndpoint, form);

            // Lire la réponse
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonResult = JsonConvert.DeserializeObject(jsonResponse);
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Response: {JsonConvert.SerializeObject(jsonResult, Formatting.Indented)}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }
 */

/*
 
{
	"query": {
		"project": "best",
		"images": [
			"buffer_code_image_1",
			"buffer_code_image_2"
		],
		"organs": [
			"flower",
			"leaf"
		]
	},
	"language": "en",
	"preferedReferential": "useful",
	"results": [
		{
			"score": 0.9952006530761719,
			"species": {
				"scientificNameWithoutAuthor": "Hibiscus rosa-sinensis",
				"scientificNameAuthorship": "L.",
				"genus": {
					"scientificNameWithoutAuthor": "Hibiscus",
					"scientificNameAuthorship": "L."
				},
				"family": {
					"scientificNameWithoutAuthor": "Malvaceae",
					"scientificNameAuthorship": "Juss."
				},
				"commonNames": [
					"Chinese hibiscus",
					"Hawaiian hibiscus",
					"Hibiscus"
				]
			}
		}
	],
	"remainingIdentificationRequests": 1228
}
 */
