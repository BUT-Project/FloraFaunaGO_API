using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Shared.Interfaces;

public interface IImageProcessingService
{
    Task<byte[]> ResizeAsync(byte[] imageData, int maxWidth, int maxHeight);
    Task<byte[]> CompressAsync(byte[] imageData, int quality = 85);
    bool IsValidImageFormat(IFormFile file);
    Task<byte[]> GenerateThumbnailAsync(byte[] imageData, int size = 150);
    string GetContentType(string fileName);
}