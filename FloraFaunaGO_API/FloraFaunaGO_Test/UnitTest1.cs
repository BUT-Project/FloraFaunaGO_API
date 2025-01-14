using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFaunaGO_Entities.Enum;

namespace FloraFaunaGO_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ToEntities_Should_Map_FullEspeceDto_To_EspeceEntities()
        {
            // Arrange
            var especeDto = new FullEspeceDto
            {
                Espece = new EspeceNormalDto { /* Remplissez les propriétés pertinentes */ },
                Habitats = new[] { new HabitatNormalDto() },
                Famille = new Famille { /* Remplissez les données */ },
                Regime_Alimentaire = new Regime_Alimentaire { /* Données */ },
                localisationNormalDtos = new[] { new LocalisationNormalDto() }
            };

            // Act
            var entities = especeDto.ToEntites();

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
        public void ToEntities_Should_Map_HabitatNormalDto_To_HabitatEntities()
        {
            // Arrange
            var habitatDto = new HabitatNormalDto
            {
                // Remplissez ici avec les valeurs pertinentes
            };

            // Act
            var habitatEntity = habitatDto.ToEntities();

            // Assert
            Assert.IsNotNull(habitatEntity);
            Assert.IsInstanceOfType<HabitatEntities>(habitatEntity);
            // Vérifiez ici les propriétés mappées spécifiques
        }

        [TestMethod]
        public void ToDto_Should_Map_HabitatEntities_To_HabitatNormalDto()
        {
            // Arrange
            var habitatEntity = new HabitatEntities
            {
                // Remplissez ici avec les valeurs pertinentes
            };

            // Act
            var habitatDto = habitatEntity.ToDto();

            // Assert
            Assert.IsNotNull(habitatDto);
            Assert.IsInstanceOfType<HabitatNormalDto>(habitatDto);
            // Testez les propriétés
        }

        [TestMethod]
        public void ToEntities_Should_Map_FullCaptureDto_To_CaptureEntities()
        {
            // Arrange
            var captureDto = new FullCaptureDto
            {
                Capture = new CaptureNormalDto { /* Remplissez les données */ },
                Espece = new FullEspeceDto
                {
                    Espece = new EspeceNormalDto { /* Remplissez les propriétés pertinentes */ },
                    Habitats = new[] { new HabitatNormalDto() },
                    Famille = new Famille { /* Remplissez les données */ },
                    Regime_Alimentaire = new Regime_Alimentaire { /* Données */ },
                    localisationNormalDtos = new[] { new LocalisationNormalDto() }
                }
            };

            // Act
            var captureEntity = captureDto.ToEntities();

            // Assert
            Assert.IsNotNull(captureEntity);
            Assert.IsInstanceOfType<CaptureEntities>(captureEntity);
            // Vérifiez les propriétés
        }

        [TestMethod]
        public void ToDto_Should_Map_CaptureEntities_To_FullCaptureDto()
        {
            // Arrange
            var captureEntity = new CaptureEntities
            {
                // Remplissez les données ici
            };

            // Act
            var captureDto = captureEntity.ToDto();

            // Assert
            Assert.IsNotNull(captureDto);
            Assert.IsInstanceOfType<FullCaptureDto>(captureDto);
            // Vérifiez les propriétés
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
}
