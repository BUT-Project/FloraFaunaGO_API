using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;

namespace FloraFaunaGO_Test;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void ToEntities_Should_Map_FullEspeceDto_To_EspeceEntities()
    {
        // Arrange
        FullEspeceDto especeDto = new FullEspeceDto()
        {
            Nom = "Lion"
        };

        // Act
        var entities = especeDto.ToEntities();

        // Assert
        Assert.IsNotNull(entities);
        Assert.IsInstanceOfType<EspeceEntities>(entities);
        // Ajoutez des vérifications supplémentaires ici
    }

    [TestMethod]
    public void ToDto_Should_Map_EspeceEntities_To_FullEspeceDto()
    {
        // Arrange
        var especeEntity = new EspeceEntities
        {
            // Remplissez les propriétés nécessaires
        };

        // Act
        var dtos = especeEntity.ToDto();

        // Assert
        Assert.IsNotNull(dtos);
        Assert.IsInstanceOfType<FullEspeceDto>(dtos);
        // Vérifiez les propriétés spécifiques, si besoin
    }

    [TestMethod]
    public void ToEntities_Should_Map_FullCaptureDto_To_CaptureEntities()
    {
        var captureDto = new FullCaptureDto
        {
            Capture = new CaptureNormalDto { 
                photo = new byte[] { 1, 2, 3 }
            },
            
        };

        // Act
        var captureEntity = captureDto.Capture.ToEntities();

        // Assert
        Assert.IsNotNull(captureEntity);
        Assert.IsInstanceOfType<CaptureEntities>(captureEntity);
        Assert.AreEqual(captureEntity.Photo, captureDto.Capture.photo);
        // Vérifiez les propriétés
    }

    [TestMethod]
    public void ToDto_Should_Map_CaptureEntities_To_FullCaptureDto()
    {
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
                    Photo = new byte[] { },
                    Espece = new EspeceEntities
                    {
                        Id = "2",
                        Description = "2",
                        Image3D = new byte[] { },
                        Famille = "Félin",
                        Image = new byte[] { },
                        Nom = "Nom",
                        Nom_scientifique = "nom science",
                        Localisations = new List<EspeceLocalisationEntities>()
                    }
                };

        // Act
        var captureDto = captureEntity.ToDto();

        // Assert
        Assert.IsNotNull(captureDto);
        Assert.IsInstanceOfType<CaptureNormalDto>(captureDto);
    }

    [TestMethod]
    public void Mapper_Should_Store_And_Return_Correct_Mappings()
    {
        // Arrange
        var mapper = new Mapper<FullEspeceDto, EspeceEntities>();
        var dto = new FullEspeceDto { /* Initialise les propriétés */ };
        var entity = new EspeceEntities { /* Initialise les propriétés */ };

        // Act
        mapper.AddMapping(dto, entity);

        // Assert
        var returnedDto = mapper.GetT(entity);
        var returnedEntity = mapper.GetU(dto);

        Assert.AreEqual<FullEspeceDto?>(dto, returnedDto);
        Assert.AreEqual<EspeceEntities>(entity, returnedEntity);
    }

    [TestMethod]
    public void Mapper_Reset_Should_Clear_All_Mappings()
    {
        // Arrange
        var mapper = new Mapper<FullEspeceDto, EspeceEntities>();
        var dto = new FullEspeceDto { /* Initialise les propriétés */ };
        var entity = new EspeceEntities { /* Initialise les propriétés */ };
        mapper.AddMapping(dto, entity);

        // Act
        mapper.Reset();

        // Assert
        Assert.IsNull(mapper.GetT(entity));
        Assert.IsNull(mapper.GetU(dto));
    }

}


