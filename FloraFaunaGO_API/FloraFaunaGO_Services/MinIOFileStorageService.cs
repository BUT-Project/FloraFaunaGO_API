using FloraFauna_GO_Shared.Configuration;
using FloraFauna_GO_Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace FloraFaunaGO_Services;

public class MinIOFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinIOConfiguration _config;
    private readonly ILogger<MinIOFileStorageService> _logger;

    public MinIOFileStorageService(
        IMinioClient minioClient,
        IOptions<MinIOConfiguration> config,
        ILogger<MinIOFileStorageService> logger)
    {
        _minioClient = minioClient;
        _config = config.Value;
        _logger = logger;
    }

    public async Task<string> UploadAsync(Microsoft.AspNetCore.Http.IFormFile file, string folder)
    {
        try
        {
            _logger.LogInformation("====== UPLOADING TO MINIO: {FileName} ======", file.FileName);
            _logger.LogInformation("File size: {FileSize} bytes", file.Length);
            
            await EnsureBucketExistsAsync();

            var fileName = GenerateUniqueFileName(file.FileName);
            var objectName = string.IsNullOrEmpty(folder) ? fileName : $"{folder}/{fileName}";
            
            _logger.LogInformation("Generated object name: {ObjectName}", objectName);

            using var stream = file.OpenReadStream();
            _logger.LogInformation("Stream opened - CanRead: {CanRead}, CanSeek: {CanSeek}, Length: {Length}, Position: {Position}", 
                stream.CanRead, stream.CanSeek, 
                stream.CanSeek ? stream.Length : -1, 
                stream.CanSeek ? stream.Position : -1);
            
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(GetContentType(file.FileName));

            _logger.LogInformation("Calling MinIO PutObjectAsync with bucket: {Bucket}, object: {Object}, size: {Size}", 
                _config.BucketName, objectName, file.Length);
            
            await _minioClient.PutObjectAsync(putObjectArgs);

            _logger.LogInformation("File {FileName} uploaded successfully to {ObjectName}", fileName, objectName);
            
            return objectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName}", file.FileName);
            throw;
        }
    }

    public async Task<Stream> DownloadAsync(string fileName)
    {
        try
        {
            _logger.LogInformation("====== DOWNLOADING FROM MINIO: {FileName} ======", fileName);
            _logger.LogInformation("Bucket: {BucketName}, Object: {ObjectName}", _config.BucketName, fileName);
            
            var stream = new MemoryStream();
            
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName)
                .WithCallbackStream(s => {
                    _logger.LogInformation("MinIO callback stream - CanRead: {CanRead}, Length: {Length}", s.CanRead, s.CanSeek ? s.Length : -1);
                    s.CopyTo(stream);
                    _logger.LogInformation("After copy - MemoryStream Length: {Length}", stream.Length);
                });

            _logger.LogInformation("Calling MinIO GetObjectAsync...");
            await _minioClient.GetObjectAsync(getObjectArgs);
            
            _logger.LogInformation("MinIO GetObjectAsync completed - MemoryStream final length: {Length}", stream.Length);
            stream.Position = 0;
            
            return stream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {FileName}", fileName);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string fileName)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs);
            
            _logger.LogInformation("File {FileName} deleted successfully", fileName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FileName}", fileName);
            return false;
        }
    }

    public async Task<string> GetPresignedUrlAsync(string fileName, TimeSpan expiry)
    {
        try
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName)
                .WithExpiry((int)expiry.TotalSeconds);

            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating presigned URL for file {FileName}", fileName);
            throw;
        }
    }

    public async Task<bool> FileExistsAsync(string fileName)
    {
        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName);

            await _minioClient.StatObjectAsync(statObjectArgs);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task EnsureBucketExistsAsync()
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(_config.BucketName);

        var exists = await _minioClient.BucketExistsAsync(bucketExistsArgs);
        
        if (!exists)
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(_config.BucketName);

            await _minioClient.MakeBucketAsync(makeBucketArgs);
            _logger.LogInformation("Bucket {BucketName} created successfully", _config.BucketName);
        }
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var uniqueName = $"{Guid.NewGuid()}{extension}";
        return uniqueName;
    }

    private static string GetContentType(string fileName)
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