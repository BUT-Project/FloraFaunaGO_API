﻿using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraFaunaGO_API.Controllers;

[Authorize]
[ApiController]
[Route("FloraFaunaGo_API/identification")]
public class IdentificationController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    private readonly ILogger<IdentificationController> _logger;
    public IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> UnitOfWork { get; private set; }
    public IdentificationService Service { get; private set; }

    public IdentificationController(ILogger<CaptureController> logger, FloraFaunaService service, IWebHostEnvironment env)
    {
        UnitOfWork = service;
        Service = new IdentificationService(UnitOfWork.EspeceRepository);
        _env = env;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullEspeceDto>> AskToIdentifyAPI(string? especeType, [FromBody] AnimalIdentifyNormalDto dto)
    {
        if (especeType is null) especeType = "Plant";
        if (!Enum.TryParse(especeType, true, out EspeceType type))
            return BadRequest("Action invalide.");

        var result = await Service.identify(dto, type);
        if (result is not null && result.Id is null)
        {
            UnitOfWork.AddEspeceAsync(result, result.localisations);
            var inserted = await UnitOfWork.SaveChangesAsync();
            result = UnitOfWork.EspeceRepository.GetEspeceByName(result.Nom).Result.Items.FirstOrDefault();
        }

        if (result is not null)
        {
            result.localisations = UnitOfWork.LocalisationRepository.GetLocalisationByEspece(result.Id).Result.Items.ToArray();
        }

        return result != null ? Ok(result) : NoContent();
    }
}
