using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;

namespace FloraFaunaGO_Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void ToEntities_Should_Map_FullEspeceDto_To_EspeceEntities()
        {
            // Arrange
            FullEspeceDto especeDto = new FullEspeceDto
            {
                Nom = "Lion",
                Nom_Scientifique = "Panthera leo",
                localisations = new[] { new LocalisationNormalDto { Latitude = 1.0, Longitude = 2.0, Rayon = 3.0 } }
            };

            // Act
            var entities = especeDto.ToEntities();

            // Assert
            Assert.IsNotNull(entities);
            Assert.IsInstanceOfType(entities, typeof(EspeceEntities));
            Assert.AreEqual(especeDto.Nom, entities.Nom);
            Assert.AreEqual(especeDto.Nom_Scientifique, entities.Nom_scientifique);
        }

        [TestMethod]
        public void ToDto_Should_Map_EspeceEntities_To_FullEspeceDto()
        {
            // Arrange
            var especeEntity = new EspeceEntities
            {
                Nom = "Lion",
                Nom_scientifique = "Panthera leo",
                Localisations = new List<EspeceLocalisationEntities> { new EspeceLocalisationEntities { } }
            };

            // Act
            var dto = especeEntity.ToDto();

            // Assert
            Assert.IsNotNull(dto);
            Assert.IsInstanceOfType(dto, typeof(FullEspeceDto));
            Assert.AreEqual(especeEntity.Nom, dto.Nom);
            Assert.AreEqual(especeEntity.Nom_scientifique, dto.Nom_Scientifique);
        }

        [TestMethod]
        public void ToEntities_Should_Map_FullCaptureDto_To_CaptureEntities()
        {
            // Arrange
            var captureDto = new FullCaptureDto
            {
                Capture = new FloraFauna_GO_Dto.Edit.ResponseCaptureDto { Id = "1", photo = new byte[] { 1, 2, 3 } },
                CaptureDetails = []
            };

            var capture = new CaptureNormalDto
            {
                Id = captureDto.Capture.Id,
                photo = captureDto.Capture.photo,
            };

            // Act
            var captureEntity = capture.ToEntities();
            captureEntity.CaptureDetails = captureDto.CaptureDetails.Select(cd => cd.CaptureDetail.ToEntities()).ToList();

            // Assert
            Assert.IsNotNull(captureEntity);
            Assert.IsInstanceOfType(captureEntity, typeof(CaptureEntities));
            Assert.AreEqual(captureDto.Capture.photo, captureEntity.Photo);
            Assert.AreEqual(captureDto.CaptureDetails.Count, captureEntity.CaptureDetails.Count);
        }

        [TestMethod]
        public void ToDto_Should_Map_CaptureEntities_To_FullCaptureDto()
        {
            // Arrange
            var captureEntity = new CaptureEntities
            {
                Id = "1",
                Photo = new byte[] { 1, 2, 3 },
                Espece = new EspeceEntities
                {
                    Nom = "Lion",
                    Nom_scientifique = "Panthera leo",
                    Localisations = new List<EspeceLocalisationEntities> { }
                },
                CaptureDetails = new List<CaptureDetailsEntities>
                {
                    new CaptureDetailsEntities
                    {
                        Shiny = true,
                        Localisation = new LocalisationEntities { Latitude = 1.0, Longitude = 2.0, Rayon = 3.0 }
                    }
                }
            };

            // Act
            var captureDto = captureEntity.ToDto();

            // Assert
            Assert.IsNotNull(captureDto);
            Assert.IsInstanceOfType(captureDto, typeof(CaptureNormalDto));
            Assert.AreEqual(captureEntity.Photo, captureDto.photo);
        }

        [TestMethod]
        public void Mapper_Should_Store_And_Return_Correct_Mappings()
        {
            // Arrange
            var mapper = new Mapper<EspeceNormalDto, EspeceEntities>();
            var dto = new EspeceNormalDto { Nom = "Lion" };
            var entity = new EspeceEntities { Nom = "Lion" };

            // Act
            mapper.AddMapping(dto, entity);

            // Assert
            var returnedDto = mapper.GetT(entity);
            var returnedEntity = mapper.GetU(dto);

            Assert.AreEqual(dto, returnedDto);
            Assert.AreEqual(entity, returnedEntity);
        }

        [TestMethod]
        public void Mapper_Reset_Should_Clear_All_Mappings()
        {
            // Arrange
            var mapper = new Mapper<EspeceNormalDto, EspeceEntities>();
            var dto = new EspeceNormalDto { Nom = "Lion" };
            var entity = new EspeceEntities { Nom = "Lion" };
            mapper.AddMapping(dto, entity);

            // Act
            mapper.Reset();

            // Assert
            Assert.IsNull(mapper.GetT(entity));
            Assert.IsNull(mapper.GetU(dto));
        }
    }
}