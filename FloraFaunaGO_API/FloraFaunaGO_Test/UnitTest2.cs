using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFaunaGO_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                Espece = new EspeceNormalDto { Nom = "Lion", Nom_Scientifique = "Panthera leo" },
                localisationNormalDtos = new[] { new LocalisationNormalDto { Latitude = 1.0, Longitude = 2.0, Rayon = 3.0 } }
            };

            // Act
            var entities = especeDto.ToEntities();

            // Assert
            Assert.IsNotNull(entities);
            Assert.IsInstanceOfType(entities, typeof(EspeceEntities));
            Assert.AreEqual(especeDto.Espece.Nom, entities.Nom);
            Assert.AreEqual(especeDto.Espece.Nom_Scientifique, entities.Nom_scientifique);
            Assert.AreEqual(especeDto.localisationNormalDtos.Length, entities.Localisations.Count);
        }

        [TestMethod]
        public void ToDto_Should_Map_EspeceEntities_To_FullEspeceDto()
        {
            // Arrange
            var especeEntity = new EspeceEntities
            {
                Nom = "Lion",
                Nom_scientifique = "Panthera leo",
                Localisations = new List<LocalisationEntities> { new LocalisationEntities { Latitude = 1.0, Longitude = 2.0, Rayon = 3.0 } }
            };

            // Act
            var dto = especeEntity.ToDto();

            // Assert
            Assert.IsNotNull(dto);
            Assert.IsInstanceOfType(dto, typeof(FullEspeceDto));
            Assert.AreEqual(especeEntity.Nom, dto.Espece.Nom);
            Assert.AreEqual(especeEntity.Nom_scientifique, dto.Espece.Nom_Scientifique);
            Assert.AreEqual(especeEntity.Localisations.Count, dto.localisationNormalDtos.Length);
        }

        [TestMethod]
        public void ToEntities_Should_Map_FullCaptureDto_To_CaptureEntities()
        {
            // Arrange
            var captureDto = new FullCaptureDto
            {
                Capture = new CaptureNormalDto { Id = "1", photo = new byte[] { 1, 2, 3 } },
                Espece = new FullEspeceDto
                {
                    Espece = new EspeceNormalDto { Nom = "Lion", Nom_Scientifique = "Panthera leo" },
                    localisationNormalDtos = new[] { new LocalisationNormalDto { Latitude = 1.0, Longitude = 2.0, Rayon = 3.0 } }
                },
                CaptureDetails = new[]
                {
                    new FullCaptureDetailDto
                    {
                        CaptureDetail = new CaptureDetailNormalDto { Id = "1", Shiny = true },
                        localisationNormalDtos = new LocalisationNormalDto { Latitude = 1.0, Longitude = 2.0, Rayon = 3.0 }
                    }
                }
            };

            // Act
            var captureEntity = captureDto.ToEntities();

            // Assert
            Assert.IsNotNull(captureEntity);
            Assert.IsInstanceOfType(captureEntity, typeof(CaptureEntities));
            Assert.AreEqual(captureDto.Capture.Id, captureEntity.Id);
            Assert.AreEqual(captureDto.Capture.photo, captureEntity.Photo);
            Assert.AreEqual(captureDto.CaptureDetails.Length, captureEntity.CaptureDetails.Count);
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
                    Localisations = new List<LocalisationEntities> { new LocalisationEntities { Latitude = 1.0, Longitude = 2.0, Rayon = 3.0 } }
                },
                CaptureDetails = new List<CaptureDetailsEntities>
                {
                    new CaptureDetailsEntities
                    {
                        Id = "1",
                        Shiny = true,
                        Localisation = new LocalisationEntities { Latitude = 1.0, Longitude = 2.0, Rayon = 3.0 }
                    }
                }
            };

            // Act
            var captureDto = captureEntity.ToDto();

            // Assert
            Assert.IsNotNull(captureDto);
            Assert.IsInstanceOfType(captureDto, typeof(FullCaptureDto));
            Assert.AreEqual(captureEntity.Id, captureDto.Capture.Id);
            Assert.AreEqual(captureEntity.Photo, captureDto.Capture.photo);
            Assert.AreEqual(captureEntity.CaptureDetails.Count, captureDto.CaptureDetails.Length);
        }

        [TestMethod]
        public void Mapper_Should_Store_And_Return_Correct_Mappings()
        {
            // Arrange
            var mapper = new Mapper<FullEspeceDto, EspeceEntities>();
            var dto = new FullEspeceDto { Espece = new EspeceNormalDto { Nom = "Lion" } };
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
            var mapper = new Mapper<FullEspeceDto, EspeceEntities>();
            var dto = new FullEspeceDto { Espece = new EspeceNormalDto { Nom = "Lion" } };
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
