using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Shared.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(IFormFile file, string folder);
    Task<Stream> DownloadAsync(string fileName);
    Task<bool> DeleteAsync(string fileName);
    Task<string> GetPresignedUrlAsync(string fileName, TimeSpan expiry);
    Task<bool> FileExistsAsync(string fileName);
}