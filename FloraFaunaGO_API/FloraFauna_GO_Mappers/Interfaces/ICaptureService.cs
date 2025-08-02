using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Mappers.Interfaces;

/// <summary>
/// Facade service that orchestrates the complete capture creation process.
/// This service provides a simplified interface for the controller and manages
/// the entire workflow: file upload, species identification, and database persistence.
/// </summary>
public interface ICaptureService
{
    /// <summary>
    /// Creates a complete capture from an uploaded image.
    /// This method orchestrates: image upload, species identification, and database transaction.
    /// </summary>
    /// <param name="image">The uploaded image file</param>
    /// <param name="userId">The ID of the user creating the capture</param>
    /// <param name="type">The type of species to identify</param>
    /// <returns>The created capture with all related data</returns>
    Task<FullCaptureDto?> CreateCaptureFromImageAsync(IFormFile image, string userId, EspeceType type);
}