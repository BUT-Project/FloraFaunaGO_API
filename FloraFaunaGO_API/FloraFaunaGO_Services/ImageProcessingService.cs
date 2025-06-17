using FloraFauna_GO_Shared.Interfaces;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace FloraFaunaGO_Services;

public class ImageProcessingService : IImageProcessingService
{
    private readonly ILogger<ImageProcessingService> _logger;
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
    private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png", "image/webp", "image/gif" };

    public ImageProcessingService(ILogger<ImageProcessingService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> ResizeAsync(byte[] imageData, int maxWidth, int maxHeight)
    {
        try
        {
            using var image = Image.Load(imageData);
            
            // Calculate new dimensions while maintaining aspect ratio
            var aspectRatio = (double)image.Width / image.Height;
            int newWidth, newHeight;

            if (aspectRatio > 1) // Landscape
            {
                newWidth = Math.Min(maxWidth, image.Width);
                newHeight = (int)(newWidth / aspectRatio);
            }
            else // Portrait or square
            {
                newHeight = Math.Min(maxHeight, image.Height);
                newWidth = (int)(newHeight * aspectRatio);
            }

            image.Mutate(x => x.Resize(newWidth, newHeight));

            using var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, new JpegEncoder { Quality = 85 });
            
            return outputStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resizing image");
            throw;
        }
    }

    public async Task<byte[]> CompressAsync(byte[] imageData, int quality = 85)
    {
        try
        {
            using var image = Image.Load(imageData);
            using var outputStream = new MemoryStream();
            
            await image.SaveAsync(outputStream, new JpegEncoder { Quality = quality });
            
            return outputStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error compressing image");
            throw;
        }
    }

    public bool IsValidImageFormat(Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        // Check file extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return false;

        // Check MIME type
        if (!AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            return false;

        // Try to load the image to verify it's a valid image
        try
        {
            using var stream = file.OpenReadStream();
            using var image = Image.Load(stream);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<byte[]> GenerateThumbnailAsync(byte[] imageData, int size = 150)
    {
        try
        {
            using var image = Image.Load(imageData);
            
            // Create square thumbnail
            var minDimension = Math.Min(image.Width, image.Height);
            var cropWidth = minDimension;
            var cropHeight = minDimension;
            var cropX = (image.Width - cropWidth) / 2;
            var cropY = (image.Height - cropHeight) / 2;

            image.Mutate(x => x
                .Crop(new Rectangle(cropX, cropY, cropWidth, cropHeight))
                .Resize(size, size));

            using var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, new JpegEncoder { Quality = 85 });
            
            return outputStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating thumbnail");
            throw;
        }
    }

    public string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}