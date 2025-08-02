using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Interfaces;
using FloraFauna_GO_Mappers.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Mappers;

/// <summary>
/// Facade service that orchestrates the complete capture creation process.
/// This service acts as the single entry point for capture creation and manages
/// the coordination between file storage, species identification, and database persistence.
/// </summary>
public class CaptureService : ICaptureService
{
    private readonly IIdentificationService _identificationService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IEspeceRepository<FullEspeceDto, FullEspeceDto> especeRepository;
    private readonly ICaptureRepository<CaptureNormalDto, FullCaptureDto> captureRepository;
    public CaptureService(
        IIdentificationService identificationService,
        IFileStorageService fileStorageService,
        FloraFaunaService unitOfWork)
    {
        _identificationService = identificationService;
        _fileStorageService = fileStorageService;
        
        especeRepository = unitOfWork.EspeceRepository;
        captureRepository = unitOfWork.CaptureRepository;
    }

    public async Task<FullCaptureDto?> CreateCaptureFromImageAsync(IFormFile image, string userId, EspeceType type)
    {
            Console.WriteLine($"Starting capture creation for user {userId}, species type: {type}");

            // Step 1: Convert IFormFile to byte array for identification
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            // Step 2: Upload the capture image and get URL
            var captureImageUrl = await _fileStorageService.UploadAsync(image, "captures/");
            Console.WriteLine($"Capture image uploaded: {captureImageUrl}");

            // Step 3: Identify the species using our identification service
            var identifiedSpecies = await _identificationService.IdentifySpeciesAsync(imageBytes, type);
            if (identifiedSpecies == null)
            {
                Console.WriteLine("Species identification failed");
                return null;
            }

            // Step 4: Handle new species persistence if needed
            if (string.IsNullOrEmpty(identifiedSpecies.Id))
            {
                Console.WriteLine($"Adding new species to database: {identifiedSpecies.Nom}");
                var insertedSpecies = await especeRepository.Insert(identifiedSpecies);
                if (insertedSpecies != null)
                {
                    identifiedSpecies = insertedSpecies;
                }
            }

            // Step 5: Create the capture entity
            var newCapture = new CaptureNormalDto
            {
                Id = null, // Will be set by database
                IdEspece = identifiedSpecies.Id!,
                photoUrl = captureImageUrl,
                LocalisationNormalDto = null, // Can be added later if needed [TODO: Implement location handling]
                Shiny = false // Default value
            };

            // Step 6: Add capture to database
            var insertedCapture = await captureRepository.Insert(newCapture);
            if (insertedCapture == null)
            {
                Console.WriteLine("Failed to insert capture into database");
                return null;
            }
            
            Console.WriteLine($"Capture created successfully with ID: {insertedCapture.Capture.Id}");

            // Step 8: Return the complete capture DTO
            return insertedCapture;
    }
}