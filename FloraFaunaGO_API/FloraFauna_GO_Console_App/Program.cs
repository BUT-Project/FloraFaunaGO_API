// See https://aka.ms/new-console-template for more information
using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;

var captureDto = new FullCaptureDto
{
    Capture = new CaptureNormalDto { /* Remplissez les données */ },
    Espece = new FullEspeceDto
    {
        Espece = new EspeceNormalDto { /* Remplissez les propriétés pertinentes */ },
        localisationNormalDtos = new[] { new LocalisationNormalDto() }
    },
    CaptureDetails = new[]
            {
                new FullCaptureDetailDto() {
                    CaptureDetail = new CaptureDetailNormalDto {
                        Id = "1",
                        Shiny = false,
                    },
                    localisationNormalDtos = new LocalisationNormalDto()
                       {
                           Latitude = 0,
                           Longitude = 0,
                           Rayon = 0,
                       }
                }
            }
};

// Act
var captureEntity = captureDto.ToEntities();

// Assert
if (captureEntity.CaptureDetails.First().Shiny == captureDto.CaptureDetails.First().CaptureDetail.Shiny)
{
    Console.WriteLine("Test Passed");
}
else
{
    Console.WriteLine("Test Failed");
}
