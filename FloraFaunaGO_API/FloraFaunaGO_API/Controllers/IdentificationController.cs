using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Mappers.Interfaces;
using FloraFauna_GO_Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FloraFaunaGO_API.Controllers;

[Authorize]
[ApiController]
[Route("FloraFaunaGo_API/identification")]
public class IdentificationController : ControllerBase
{
    private readonly ILogger<IdentificationController> _logger;
    private readonly ICaptureService _captureService;
    private readonly IIdentificationService _identificationService;
    private readonly IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto> _unitOfWork;

    public IdentificationController(
        ILogger<IdentificationController> logger, 
        ICaptureService captureService,
        IIdentificationService identificationService)
    {
        _logger = logger;
        _captureService = captureService;
        _identificationService = identificationService;
    }

    /// <summary>
    /// Identifies a species from an uploaded image without creating a capture
    /// </summary>
    [HttpPost("identify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FullEspeceDto>> IdentifySpecies(string? especeType, [FromForm] AnimalIdentifyNormalDto dto)
    {
        if (especeType is null) especeType = "Plant";
        if (!Enum.TryParse(especeType, true, out EspeceType type))
            return BadRequest("Invalid species type.");

        if (dto.AskedImage == null)
            return BadRequest("Image is required.");

        try
        {
            // Convert IFormFile to byte array
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await dto.AskedImage.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            var result = await _identificationService.IdentifySpeciesAsync(imageBytes, type);
            return result != null ? Ok(result) : NotFound("No species identified from the provided image.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error identifying species");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error processing identification request");
        }
    }

    /// <summary>
    /// Creates a capture with automatic species identification
    /// </summary>
    [HttpPost("capture")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FullCaptureDto>> CreateCaptureWithIdentification(string? especeType, [FromForm] AnimalIdentifyNormalDto dto)
    {
        if (especeType is null) especeType = "Plant";
        if (!Enum.TryParse(especeType, true, out EspeceType type))
            return BadRequest("Invalid species type.");

        if (dto.AskedImage == null)
            return BadRequest("Image is required.");

        // Get user ID from JWT token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        try
        {
            var result = await _captureService.CreateCaptureFromImageAsync(dto.AskedImage, userId, type);
            // Save all changes in a single transaction
            var savedChanges = await _unitOfWork.SaveChangesAsync();
            
            if (savedChanges == null || !savedChanges.Any() || result == null )
            {
                Console.WriteLine("Failed to save changes to database");
                return BadRequest("Failed to create capture. Species identification may have failed.");
            }

            return CreatedAtAction(nameof(CreateCaptureWithIdentification), new { id = result.Capture.Id }, result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating capture: {ex.Message}");
            _logger.LogError(ex, "Error creating capture with identification for user {UserId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error processing capture request");
        }
    }
}
