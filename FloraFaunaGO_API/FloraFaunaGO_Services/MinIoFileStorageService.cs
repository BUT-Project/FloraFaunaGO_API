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
            _logger.LogCritical("UPLOAD METHOD CALLED - NEW CODE VERSION!");
            _logger.LogInformation("====== UPLOADING TO MINIO: {FileName} ======", file.FileName);
            _logger.LogInformation("File size: {FileSize} bytes", file.Length);

            _logger.LogInformation("DEBUG: About to call EnsureBucketExistsAsync...");
            await EnsureBucketExistsAsync();
            _logger.LogInformation("DEBUG: EnsureBucketExistsAsync completed");

            var fileName = GenerateUniqueFileName(file.FileName);
            var objectName = string.IsNullOrEmpty(folder) ? fileName : $"{folder}/{fileName}";

            Console.WriteLine($"Generated object name: {objectName}");

            // IMPORTANT: Copy the stream to a new MemoryStream to ensure we have all the data
            using var memoryStream = new MemoryStream();
            await using var fileStream = file.OpenReadStream();

            // Copy the file stream to memory stream
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset position after copying

            Console.WriteLine(
                $"MemoryStream created - Length: {memoryStream.Length}, Position: {memoryStream.Position}");

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(objectName)
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length) // Use actual stream length
                .WithContentType(GetContentType(file.FileName));

            Console.WriteLine(
                $"Calling MinIO PutObjectAsync with bucket: {_config.BucketName}, object: {objectName}, size: {memoryStream.Length}");

            try
            {
                await _minioClient.PutObjectAsync(putObjectArgs);
                Console.WriteLine("MinIO PutObjectAsync completed successfully");

                // Verify the upload by checking if a file exists and getting its size
                Console.WriteLine("Verifying upload by checking file existence...");
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(_config.BucketName)
                    .WithObject(objectName);

                var objectStat = await _minioClient.StatObjectAsync(statObjectArgs);
                Console.WriteLine($"Upload verified - Object size in MinIO: {objectStat.Size} bytes");

                if (objectStat.Size != file.Length)
                {
                    Console.WriteLine($"SIZE MISMATCH! Expected: {file.Length}, Actual: {objectStat.Size}");

                    // If size is 0, throw an exception to retry
                    if (objectStat.Size == 0)
                    {
                        throw new InvalidOperationException(
                            $"File uploaded with 0 bytes. Expected {file.Length} bytes.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR during MinIO PutObjectAsync: {ex}");
                throw;
            }

            Console.WriteLine($"File {fileName} uploaded successfully to {objectName}");

            return objectName;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file {file.FileName}: {ex}");
            throw;
        }
    }

    public async Task<Stream> DownloadAsync(string fileName)
    {
        try
        {
            Console.WriteLine($"====== DOWNLOADING FROM MINIO: {fileName} ======");
            Console.WriteLine($"Bucket: {_config.BucketName}, Object: {fileName}");

            var stream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName)
                .WithCallbackStream(s =>
                {
                    Console.WriteLine(
                        $"MinIO callback stream - CanRead: {s.CanRead}, Length: {(s.CanSeek ? s.Length : -1)}");
                    s.CopyTo(stream);
                    Console.WriteLine($"After copy - MemoryStream Length: {stream.Length}");
                });

            Console.WriteLine("Calling MinIO GetObjectAsync...");
            await _minioClient.GetObjectAsync(getObjectArgs);

            Console.WriteLine($"MinIO GetObjectAsync completed - MemoryStream final length: {stream.Length}");
            stream.Position = 0;

            return stream;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file {fileName}: {ex}");
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

            Console.WriteLine($"File {fileName} deleted successfully");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file {fileName}: {ex}");
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
            Console.WriteLine($"Error generating presigned URL for file {fileName}: {ex}");
            throw;
        }
    }

    public async Task<bool> FileExistsAsync(string fileName)
    {
        try
        {
            _logger.LogInformation("====== CHECKING FILE EXISTS: {FileName} ======", fileName);
            _logger.LogInformation("Bucket: {BucketName}, Object: {ObjectName}", _config.BucketName, fileName);

            // List bucket contents to debug
            _logger.LogInformation("Listing bucket contents for debugging:");
            try
            {
                var listObjectsArgs = new ListObjectsArgs()
                    .WithBucket(_config.BucketName)
                    .WithPrefix("uploads/")
                    .WithRecursive(true);

                var objects = _minioClient.ListObjectsAsync(listObjectsArgs);
                await objects.ForEachAsync(obj =>
                {
                    _logger.LogInformation("Found object: {ObjectKey} (Size: {Size})", obj.Key, obj.Size);
                });
            }
            catch (Exception listEx)
            {
                _logger.LogError("Error listing objects: {Error}", listEx.Message);
            }

            var statObjectArgs = new StatObjectArgs()
                .WithBucket(_config.BucketName)
                .WithObject(fileName);

            await _minioClient.StatObjectAsync(statObjectArgs);
            _logger.LogInformation("File exists: {FileName}", fileName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("File does not exist or error checking: {FileName}, Error: {Error}", fileName,
                ex.Message);
            _logger.LogError("Full exception: {Exception}", ex);
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
            Console.WriteLine($"ERROR: Failed to connect to MinIO or check bucket existence: {ex}");
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