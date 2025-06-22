// See https://aka.ms/new-console-template for more information
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;

var captureDto = new CaptureDetailNormalDto
{
    Id = "1",
    Shiny = false,
};

// Act
var captureEntity = captureDto.ToEntities();
