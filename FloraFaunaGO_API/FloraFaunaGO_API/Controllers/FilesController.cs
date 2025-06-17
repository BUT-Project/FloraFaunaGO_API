using FloraFauna_GO_Shared.Interfaces;
using FloraFauna_GO_Shared.Services;
using FloraFauna_GO_Shared.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace FloraFaunaGO_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly FileValidationService _fileValidationService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(
        IFileStorageService fileStorageService,
        IImageProcessingService imageProcessingService,
        IOptions<MinIOConfiguration> config,
        ILogger<FilesController> logger)
    {
        _fileStorageService = fileStorageService;
        _imageProcessingService = imageProcessingService;
        _fileValidationService = new FileValidationService(config.Value);
        _logger = logger;
    }

    /// <summary>
    /// Upload a single image file
    /// </summary>
    /// <param name="file">Image file to upload</param>
    /// <param name="folder">Optional folder name (captures, species, users)</param>
    /// <returns>File URL</returns>
    [HttpPost("upload")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
    public async Task<ActionResult<string>> UploadImage(
        [Required] IFormFile file,
        [FromQuery] string folder = "uploads")
    {
        try
        {
            // Validate file
            var validationResult = _fileValidationService.ValidateFile(file);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("File validation failed: {ErrorMessage}", validationResult.ErrorMessage);
                return BadRequest(validationResult.ErrorMessage);
            }

            // Upload file
            var fileName = await _fileStorageService.UploadAsync(file, folder);
            
            _logger.LogInformation("File uploaded successfully: {FileName}", fileName);
            
            return Ok(new { url = fileName, message = "File uploaded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading file");
        }
    }

    /// <summary>
    /// Serve an image file
    /// </summary>
    /// <param name="fileName">File name to serve (can include folder path)</param>
    /// <returns>Image file stream</returns>
    [HttpGet("{*fileName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ServeImage(string fileName)
    {
        try
        {
            if (!await _fileStorageService.FileExistsAsync(fileName))
            {
                return NotFound();
            }

            var stream = await _fileStorageService.DownloadAsync(fileName);
            var contentType = _imageProcessingService.GetContentType(fileName);
            
            return File(stream, contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error serving file {FileName}", fileName);
            return NotFound();
        }
    }

    /// <summary>
    /// Get a presigned URL for temporary access
    /// </summary>
    /// <param name="fileName">File name (passed as query parameter)</param>
    /// <param name="expiryMinutes">URL expiry in minutes (default: 60)</param>
    /// <returns>Presigned URL</returns>
    [HttpGet("presigned")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> GetPresignedUrl(
        [FromQuery] string fileName,
        [FromQuery] int expiryMinutes = 60)
    {
        try
        {
            if (!await _fileStorageService.FileExistsAsync(fileName))
            {
                return NotFound();
            }

            var expiry = TimeSpan.FromMinutes(expiryMinutes);
            var url = await _fileStorageService.GetPresignedUrlAsync(fileName, expiry);
            
            return Ok(new { url, expiryMinutes });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating presigned URL for {FileName}", fileName);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error generating presigned URL");
        }
    }

    /// <summary>
    /// Delete an image file
    /// </summary>
    /// <param name="fileName">File name to delete</param>
    [HttpDelete("{*fileName}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteImage(string fileName)
    {
        try
        {
            var deleted = await _fileStorageService.DeleteAsync(fileName);
            
            if (!deleted)
            {
                return NotFound();
            }

            _logger.LogInformation("File deleted successfully: {FileName}", fileName);
            return Ok(new { message = "File deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FileName}", fileName);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting file");
        }
    }
}