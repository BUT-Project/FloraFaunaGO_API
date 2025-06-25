using FloraFauna_GO_Dto.Response;
using System.Text.Json;

namespace FloraFaunaGO_Test;

[TestClass]
public class IdentificationResultDtoTests
{
    [TestMethod]
    public void PlantIdentificationResultDto_Properties_Are_Set_And_Accessible()
    {
        var dto = new PlantIdentificationResultDto
        {
            Query = new QueryDto { Project = "test", Images = new List<string> { "img1" }, Organs = new List<string> { "leaf" }, IncludeRelatedImages = true, NoReject = false, Type = null },
            PredictedOrgans = new List<PredictedOrganDto> { new PredictedOrganDto { Image = "img1", Filename = "f1", Organ = "leaf", Score = 0.9 } },
            Language = "fr",
            PreferedReferential = "ref",
            BestMatch = "rose",
            Results = new List<PlantResultDto>
            {
                new PlantResultDto
                {
                    Score = 0.95,
                    Species = new SpeciesDto
                    {
                        ScientificNameWithoutAuthor = "Rosa",
                        ScientificNameAuthorship = "L.",
                        Genus = new GenusDto { ScientificName = "Rosa", ScientificNameWithoutAuthor = "Rosa", ScientificNameAuthorship = "L." },
                        Family = new FamilyDto { ScientificName = "Rosaceae", ScientificNameWithoutAuthor = "Rosaceae", ScientificNameAuthorship = "Juss." },
                        CommonNames = new List<string> { "Rose" },
                        ScientificName = "Rosa gallica"
                    },
                    Gbif = new IdentifierDto { Id = "123" },
                    Powo = new IdentifierDto { Id = "456" },
                    Iucn = new IucnDto { Id = "iucn1", Category = "LC" }
                }
            },
            Version = "1.0",
            RemainingIdentificationRequests = 42
        };

        Assert.AreEqual("test", dto.Query.Project);
        Assert.AreEqual("img1", dto.Query.Images[0]);
        Assert.AreEqual("leaf", dto.Query.Organs[0]);
        Assert.IsTrue(dto.Query.IncludeRelatedImages);
        Assert.IsFalse(dto.Query.NoReject);
        Assert.AreEqual("fr", dto.Language);
        Assert.AreEqual("ref", dto.PreferedReferential);
        Assert.AreEqual("rose", dto.BestMatch);
        Assert.AreEqual("img1", dto.PredictedOrgans[0].Image);
        Assert.AreEqual("leaf", dto.PredictedOrgans[0].Organ);
        Assert.AreEqual(0.9, dto.PredictedOrgans[0].Score);
        Assert.AreEqual(0.95, dto.Results[0].Score);
        Assert.AreEqual("Rosa", dto.Results[0].Species.Genus.ScientificName);
        Assert.AreEqual("Rose", dto.Results[0].Species.CommonNames[0]);
        Assert.AreEqual("123", dto.Results[0].Gbif.Id);
        Assert.AreEqual("iucn1", dto.Results[0].Iucn.Id);
        Assert.AreEqual("LC", dto.Results[0].Iucn.Category);
        Assert.AreEqual("1.0", dto.Version);
        Assert.AreEqual(42, dto.RemainingIdentificationRequests);
    }

    [TestMethod]
    public void InsectIdentificationResultDto_Properties_Are_Set_And_Accessible()
    {
        var dto = new InsectIdentificationResultDto
        {
            AccessToken = "token",
            ModelVersion = "v1",
            CustomId = "cid",
            Input = new InputDto
            {
                Latitude = 12.34,
                Longitude = 56.78,
                SimilarImages = true,
                Images = new List<string> { "imgA" },
                Datetime = new DateTime(2024, 1, 1)
            },
            Result = new InsectResultDto
            {
                Classification = new ClassificationDto
                {
                    Suggestions = new List<SuggestionDto>
                    {
                        new SuggestionDto
                        {
                            Id = "s1",
                            Name = "Papillon",
                            Probability = 0.88,
                            SimilarImages = new List<SimilarImageDto>
                            {
                                new SimilarImageDto
                                {
                                    Id = "img1",
                                    Url = "http://img1",
                                    Similarity = 0.99,
                                    UrlSmall = "http://img1/small",
                                    LicenseName = "CC",
                                    LicenseUrl = "http://license",
                                    Citation = "citation"
                                }
                            },
                            Details = new SuggestionDetailsDto
                            {
                                CommonNames = new List<string> { "Butterfly" },
                                Url = "http://species",
                                Description = new DescriptionDto { Value = "desc", Citation = "cit", LicenseName = "CC", LicenseUrl = "http://lic" },
                                Image = new ImageDto { Value = "img", Citation = "cit", LicenseName = "CC", LicenseUrl = "http://lic" },
                                Language = "fr",
                                EntityId = "ent1"
                            }
                        }
                    }
                },
                IsInsect = new IsInsectDto { Probability = 0.99, Threshold = 0.5, Binary = true }
            },
            Status = "ok",
            SlaCompliantClient = true,
            SlaCompliantSystem = false,
            Created = 123456.0,
            Completed = 123457.0
        };

        Assert.AreEqual("token", dto.AccessToken);
        Assert.AreEqual("v1", dto.ModelVersion);
        Assert.AreEqual("cid", dto.CustomId);
        Assert.AreEqual(12.34, dto.Input.Latitude);
        Assert.AreEqual(56.78, dto.Input.Longitude);
        Assert.IsTrue(dto.Input.SimilarImages);
        Assert.AreEqual("imgA", dto.Input.Images[0]);
        Assert.AreEqual(new DateTime(2024, 1, 1), dto.Input.Datetime);
        Assert.AreEqual("Papillon", dto.Result.Classification.Suggestions[0].Name);
        Assert.AreEqual(0.88, dto.Result.Classification.Suggestions[0].Probability);
        Assert.AreEqual("Butterfly", dto.Result.Classification.Suggestions[0].Details.CommonNames[0]);
        Assert.AreEqual("desc", dto.Result.Classification.Suggestions[0].Details.Description.Value);
        Assert.AreEqual("img1", dto.Result.Classification.Suggestions[0].SimilarImages[0].Id);
        Assert.AreEqual(0.99, dto.Result.IsInsect.Probability);
        Assert.AreEqual("ok", dto.Status);
        Assert.IsTrue(dto.SlaCompliantClient);
        Assert.IsFalse(dto.SlaCompliantSystem);
        Assert.AreEqual(123456.0, dto.Created);
        Assert.AreEqual(123457.0, dto.Completed);
    }

    [TestMethod]
    public void PlantIdentificationResultDto_Can_Be_Deserialized()
    {
        string json = @"{
            ""Query"": { ""Project"": ""p1"", ""Images"": [""img1""], ""Organs"": [""leaf""], ""IncludeRelatedImages"": true, ""NoReject"": false, ""Type"": null },
            ""PredictedOrgans"": [ { ""Image"": ""img1"", ""Filename"": ""f1"", ""Organ"": ""leaf"", ""Score"": 0.9 } ],
            ""Language"": ""fr"",
            ""PreferedReferential"": ""ref"",
            ""BestMatch"": ""rose"",
            ""Results"": [ { ""Score"": 0.95, ""Species"": { ""ScientificNameWithoutAuthor"": ""Rosa"", ""ScientificNameAuthorship"": ""L."", ""Genus"": { ""ScientificName"": ""Rosa"", ""ScientificNameWithoutAuthor"": ""Rosa"", ""ScientificNameAuthorship"": ""L."" }, ""Family"": { ""ScientificName"": ""Rosaceae"", ""ScientificNameWithoutAuthor"": ""Rosaceae"", ""ScientificNameAuthorship"": ""Juss."" }, ""CommonNames"": [""Rose""], ""ScientificName"": ""Rosa gallica"" }, ""Gbif"": { ""Id"": ""123"" }, ""Powo"": { ""Id"": ""456"" }, ""Iucn"": { ""Id"": ""iucn1"", ""Category"": ""LC"" } } ],
            ""Version"": ""1.0"",
            ""RemainingIdentificationRequests"": 42
        }";
        var dto = JsonSerializer.Deserialize<PlantIdentificationResultDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.IsNotNull(dto);
        Assert.AreEqual("p1", dto.Query.Project);
        Assert.AreEqual("rose", dto.BestMatch);
        Assert.AreEqual("Rosa", dto.Results[0].Species.Genus.ScientificName);
    }

    [TestMethod]
    public void InsectIdentificationResultDto_Can_Be_Deserialized()
    {
        string json = @"{
            ""AccessToken"": ""token"",
            ""ModelVersion"": ""v1"",
            ""CustomId"": ""cid"",
            ""Input"": { ""Latitude"": 12.34, ""Longitude"": 56.78, ""SimilarImages"": true, ""Images"": [""imgA""], ""Datetime"": ""2024-01-01T00:00:00"" },
            ""Result"": {
                ""Classification"": {
                    ""Suggestions"": [{
                        ""Id"": ""s1"",
                        ""Name"": ""Papillon"",
                        ""Probability"": 0.88,
                        ""SimilarImages"": [ { ""Id"": ""img1"", ""Url"": ""http://img1"", ""Similarity"": 0.99, ""UrlSmall"": ""http://img1/small"", ""LicenseName"": ""CC"", ""LicenseUrl"": ""http://license"", ""Citation"": ""citation"" } ],
                        ""Details"": { ""CommonNames"": [""Butterfly""], ""Url"": ""http://species"", ""Description"": { ""Value"": ""desc"", ""Citation"": ""cit"", ""LicenseName"": ""CC"", ""LicenseUrl"": ""http://lic"" }, ""Image"": { ""Value"": ""img"", ""Citation"": ""cit"", ""LicenseName"": ""CC"", ""LicenseUrl"": ""http://lic"" }, ""Language"": ""fr"", ""EntityId"": ""ent1"" }
                    }]
                },
                ""IsInsect"": { ""Probability"": 0.99, ""Threshold"": 0.5, ""Binary"": true }
            },
            ""Status"": ""ok"",
            ""SlaCompliantClient"": true,
            ""SlaCompliantSystem"": false,
            ""Created"": 123456.0,
            ""Completed"": 123457.0
        }";
        var dto = JsonSerializer.Deserialize<InsectIdentificationResultDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.IsNotNull(dto);
        Assert.AreEqual("token", dto.AccessToken);
        Assert.AreEqual("Papillon", dto.Result.Classification.Suggestions[0].Name);
        Assert.AreEqual("Butterfly", dto.Result.Classification.Suggestions[0].Details.CommonNames[0]);
        Assert.AreEqual(0.99, dto.Result.IsInsect.Probability);
    }
}
