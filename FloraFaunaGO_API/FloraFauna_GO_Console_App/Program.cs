// See https://aka.ms/new-console-template for more information
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFaunaGO_Entities.Enum;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Entities;

var captureEntity = new CaptureEntities()
{
    Numero = 1,
    CaptureDetails = new[]
    {
        new CaptureDetailsEntities {
            Id = "1",
            Shiny = false,
            Localisation = new LocalisationEntities
            {
                Rayon = 4,
                Latitude = 4,
                Longitude = 4,
            }
        },
    },
    Photo = new byte[] {},
    Espece = new EspeceEntities
    {
        Id = "2",
        Description = "2",
        Image3D = new byte[] {},
        Famille = Famille.Félin,
        Habitats = new[]
        {
            new HabitatEntities()
            {
                Climat = "le climat",
                Zone = "la zone",
            }
        },
        Image = new byte[] {},
        Nom = "Nom",
        Nom_scientifique = "nom science",
        Localisations = new[]
        {
            new LocalisationEntities()
            {
                Longitude = 0,
                Latitude = 0,
                Rayon = 0,
            },
        },
        Regime = Regime_Alimentaire.Végétarien
    }
};

// Act
var captureDto = captureEntity.ToDto();

// Vérifiez les propriétés
