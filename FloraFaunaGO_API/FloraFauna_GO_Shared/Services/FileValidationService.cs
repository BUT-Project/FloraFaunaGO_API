using Microsoft.AspNetCore.Http;
using FloraFauna_GO_Shared.Configuration;

namespace FloraFauna_GO_Shared.Services;

public class FileValidationService
{
    private readonly MinIOConfiguration _config;

    public FileValidationService(MinIOConfiguration config)
    {
        _config = config;
    }

    public FileValidationResult ValidateFile(IFormFile file)
    {
        var result = new FileValidationResult { IsValid = true };

        if (file == null)
        {
            result.IsValid = false;
            result.ErrorMessage = "No file provided";
            return result;
        }

        // Check file size
        if (file.Length > _config.MaxFileSize)
        {
            result.IsValid = false;
            result.ErrorMessage = $"File size ({file.Length} bytes) exceeds maximum allowed size ({_config.MaxFileSize} bytes)";
            return result;
        }

        if (file.Length == 0)
        {
            result.IsValid = false;
            result.ErrorMessage = "File is empty";
            return result;
        }

        // Check file extension
        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_config.AllowedExtensions.Contains(extension))
        {
            result.IsValid = false;
            result.ErrorMessage = $"File extension '{extension}' is not allowed. Allowed extensions: {string.Join(", ", _config.AllowedExtensions)}";
            return result;
        }

        // Check content type
        if (!IsValidImageContentType(file.ContentType))
        {
            result.IsValid = false;
            result.ErrorMessage = $"Content type '{file.ContentType}' is not a valid image type";
            return result;
        }

        return result;
    }

    private static bool IsValidImageContentType(string contentType)
    {
        var validContentTypes = new[]
        {
            "image/jpeg",
            "image/jpg", 
            "image/png",
            "image/gif",
            "image/webp",
            "image/bmp",
            "image/tiff"
        };

        return validContentTypes.Contains(contentType?.ToLowerInvariant());
    }
}

public class FileValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
}