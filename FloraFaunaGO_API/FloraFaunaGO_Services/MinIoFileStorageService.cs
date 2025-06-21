using System.Reactive.Linq;
using FloraFauna_GO_Shared.Configuration;
using FloraFauna_GO_Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;

namespace FloraFaunaGO_Services;

public class MinIoFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinIOConfiguration _config;
    private readonly ILogger<MinIoFileStorageService> _logger;

    public MinIoFileStorageService(
        IMinioClient minioClient,
        IOptions<MinIOConfiguration> config,
        ILogger<MinIoFileStorageService> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
        
        _logger.LogInformation("====== MINIO SERVICE CONSTRUCTOR DEBUG ======");
        
        if (config == null || config.Value == null)
        {
            _logger.LogError("ERROR: MinIO configuration is null or has no value");
            throw new ArgumentNullException(nameof(config), "MinIO configuration is not provided.");
        }

        _config = config.Value;
        
        _logger.LogInformation("MinIO Config in constructor:");
        _logger.LogInformation("  Endpoint: '{Endpoint}'", _config.Endpoint);
        _logger.LogInformation("  AccessKey: '{AccessKey}'", _config.AccessKey);
        _logger.LogInformation("  BucketName: '{BucketName}'", _config.BucketName);
        _logger.LogInformation("  UseSSL: {UseSSL}", _config.UseSSL);
        _logger.LogInformation("===============================================");
        
        if (string.IsNullOrEmpty(_config.BucketName))
        {
            _logger.LogError("ERROR: BucketName is null or empty. Value: '{BucketName}'", _config.BucketName);
            throw new ArgumentException("Bucket name must be provided in MinIO configuration.", nameof(config));
        }
        
        _logger.LogInformation("MinIO Service constructor completed successfully");
    }
    
    public async Task<string> UploadAsync(Microsoft.AspNetCore.Http.IFormFile file, string folder)
    {
        try
        {
            _logger.LogCritical("UPLOAD METHOD CALLED - CHUNKED UPLOAD VERSION!");
            _logger.LogInformation("====== UPLOADING TO MINIO: {FileName} ======", file.FileName);
            _logger.LogInformation("File size: {FileSize} bytes", file.Length);
            
            await EnsureBucketExistsAsync();

            var fileName = GenerateUniqueFileName(file.FileName);
            var objectName = string.IsNullOrEmpty(folder) ? fileName : $"{folder}/{fileName}";
            
            _logger.LogInformation("Generated object name: {ObjectName}", objectName);

            // Read file into byte array first to ensure we have all data
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }
            
            _logger.LogInformation("File read into byte array - Length: {Length}", fileData.Length);
            
            // Verify we actually have data
            if (fileData.Length == 0)
            {
                throw new InvalidOperationException("File data is empty after reading from IFormFile");
            }
            
            // Log first few bytes to verify data
            var firstBytes = string.Join(" ", fileData.Take(10).Select(b => b.ToString("X2")));
            _logger.LogInformation("First 10 bytes of file: {FirstBytes}", firstBytes);
            
            // Try upload with a fresh memory stream
            using (var dataStream = new MemoryStream(fileData))
            {
                dataStream.Position = 0;
                
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_config.BucketName)
                    .WithObject(objectName)
                    .WithStreamData(dataStream)
                    .WithObjectSize(fileData.Length)
                    .WithContentType(GetContentType(file.FileName));

                _logger.LogInformation("Calling MinIO PutObjectAsync:");
                _logger.LogInformation("  Bucket: {Bucket}", _config.BucketName);
                _logger.LogInformation("  Object: {Object}", objectName);
                _logger.LogInformation("  Size: {Size}", fileData.Length);
                _logger.LogInformation("  ContentType: {ContentType}", GetContentType(file.FileName));
                
                await _minioClient.PutObjectAsync(putObjectArgs);
                _logger.LogInformation("MinIO PutObjectAsync completed");
            }
            
            // Wait a moment for MinIO to process
            await Task.Delay(1000);
            
            // Verify upload with retries
            var uploaded = false;
            var retries = 3;
            
            while (!uploaded && retries > 0)
            {
                try
                {
                    var statObjectArgs = new StatObjectArgs()
                        .WithBucket(_config.BucketName)
                        .WithObject(objectName);
                    
                    var objectStat = await _minioClient.StatObjectAsync(statObjectArgs);
                    _logger.LogInformation("Upload verification - Object size: {Size} bytes", objectStat.Size);
                    
                    if (objectStat.Size > 0)
                    {
                        uploaded = true;
                        
                        // Also try to read it back to verify
                        var testStream = new MemoryStream();
                        var getObjectArgs = new GetObjectArgs()
                            .WithBucket(_config.BucketName)
                            .WithObject(objectName)
                            .WithCallbackStream(stream => stream.CopyTo(testStream));
                        
                        await _minioClient.GetObjectAsync(getObjectArgs);
                        _logger.LogInformation("Read-back verification - Retrieved {Size} bytes", testStream.Length);
                    }
                    else
                    {
                        _logger.LogWarning("Object size is still 0, retrying... ({Retries} retries left)", retries - 1);
                        await Task.Delay(1000);
                        retries--;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during verification, retrying... ({Retries} retries left)", retries - 1);
                    await Task.Delay(1000);
                    retries--;
                }
            }
            
            if (!uploaded)
            {
                // As a last resort, try a different upload method
                _logger.LogWarning("Standard upload failed, trying alternative method...");
                
                // Try with PutObjectAsync using a file path
                var tempFile = Path.GetTempFileName();
                try
                {
                    await File.WriteAllBytesAsync(tempFile, fileData);
                    
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(_config.BucketName)
                        .WithObject(objectName)
                        .WithFileName(tempFile)
                        .WithContentType(GetContentType(file.FileName));
                    
                    await _minioClient.PutObjectAsync(putObjectArgs);
                    _logger.LogInformation("Alternative upload method completed");
                }
                finally
                {
                    if (File.Exists(tempFile))
                        File.Delete(tempFile);
                }
            }
            
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
            _logger.LogInformation("Bucket: {Bucket}, Object: {Object}", _config.BucketName, fileName);
            
            var stream = new MemoryStream();
            
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName)
                .WithCallbackStream(s => {
                    _logger.LogInformation("MinIO callback stream - CanRead: {CanRead}", s.CanRead);
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
            _logger.LogInformation("====== CHECKING FILE EXISTS: {FileName} ======", fileName);
            _logger.LogInformation("Bucket: {BucketName}, Object: {ObjectName}", _config.BucketName, fileName);
            
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName);

            await _minioClient.StatObjectAsync(statObjectArgs);
            _logger.LogInformation("File exists: {FileName}", fileName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("File does not exist or error checking: {FileName}, Error: {Error}", fileName, ex.Message);
            return false;
        }
    }

    private async Task EnsureBucketExistsAsync()
    {
        try
        {
            _logger.LogInformation("====== CHECKING MINIO CONNECTIVITY ======");
            _logger.LogInformation("MinIO Endpoint: {Endpoint}", _minioClient.Config.Endpoint);
            _logger.LogInformation("Checking bucket existence for: {BucketName}", _config.BucketName);
            
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(_config.BucketName);

            _logger.LogInformation("Calling MinIO BucketExistsAsync...");
            var exists = await _minioClient.BucketExistsAsync(bucketExistsArgs);
            _logger.LogInformation("MinIO BucketExistsAsync result: {Exists}", exists);
            
            if (!exists)
            {
                _logger.LogInformation("Bucket does not exist, creating bucket: {BucketName}", _config.BucketName);
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(_config.BucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs);
                _logger.LogInformation("Bucket {BucketName} created successfully", _config.BucketName);
            }
            else
            {
                _logger.LogInformation("Bucket {BucketName} already exists", _config.BucketName);
            }
            
            _logger.LogInformation("====== MINIO CONNECTIVITY CHECK COMPLETED ======");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR: Failed to connect to MinIO or check bucket existence");
            throw;
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