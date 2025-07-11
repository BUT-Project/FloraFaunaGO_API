using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using FloraFauna_GO_Shared.Configuration;
using FloraFauna_GO_Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FloraFaunaGO_Services;

public class CloudflareR2FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly CloudflareR2Configuration _config;
    private readonly ILogger<CloudflareR2FileStorageService> _logger;

    public CloudflareR2FileStorageService(
        IAmazonS3 s3Client,
        IOptions<FileStorageOptions> options,
        ILogger<CloudflareR2FileStorageService> logger)
    {
        _s3Client = s3Client;
        _config = options.Value.CloudflareR2;
        _logger = logger;
        
        ValidateConfiguration();
    }
    
    private string GetBucketName()
    {
        return Environment.GetEnvironmentVariable("CLOUDFLARE_BUCKET_NAME") ?? _config.BucketName;
    }

    public async Task<string> UploadAsync(Microsoft.AspNetCore.Http.IFormFile file, string folder)
    {
        try
        {
            _logger.LogInformation("Uploading file {FileName} to Cloudflare R2", file.FileName);
            
            var fileName = GenerateUniqueFileName(file.FileName);
            var key = string.IsNullOrEmpty(folder) ? fileName : $"{folder}/{fileName}";
            
            // Use temporary file approach to avoid streaming issues
            var tempFile = Path.GetTempFileName();
            try
            {
                // Save file to temporary location
                using (var stream = file.OpenReadStream())
                using (var fileStream = new FileStream(tempFile, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
                
                // Upload using file path with specific configuration
                var request = new PutObjectRequest
                {
                    BucketName = GetBucketName(),
                    Key = key,
                    FilePath = tempFile,
                    ContentType = GetContentType(file.FileName),
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.None,
                    UseChunkEncoding = false
                };

                var response = await _s3Client.PutObjectAsync(request);
                
                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new InvalidOperationException($"Failed to upload file. Status: {response.HttpStatusCode}");
                }

                _logger.LogInformation("File {FileName} uploaded successfully to R2 with key {Key}", file.FileName, key);
                return key;
            }
            finally
            {
                // Clean up temporary file
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName} to Cloudflare R2", file.FileName);
            throw;
        }
    }

    public async Task<Stream> DownloadAsync(string fileName)
    {
        try
        {
            _logger.LogInformation("Downloading file {FileName} from Cloudflare R2", fileName);
            
            var request = new GetObjectRequest
            {
                BucketName = GetBucketName(),
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request);
            
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            
            _logger.LogInformation("File {FileName} downloaded successfully from R2", fileName);
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {FileName} from Cloudflare R2", fileName);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string fileName)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = GetBucketName(),
                Key = fileName
            };

            var response = await _s3Client.DeleteObjectAsync(request);
            
            _logger.LogInformation("File {FileName} deleted successfully from R2", fileName);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FileName} from Cloudflare R2", fileName);
            return false;
        }
    }

    public async Task<string> GetPresignedUrlAsync(string fileName, TimeSpan expiry)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = GetBucketName(),
                Key = fileName,
                Expires = DateTime.UtcNow.Add(expiry),
                Verb = HttpVerb.GET
            };

            var url = await _s3Client.GetPreSignedURLAsync(request);
            
            _logger.LogInformation("Generated presigned URL for file {FileName}", fileName);
            return url;
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
            var request = new GetObjectMetadataRequest
            {
                BucketName = GetBucketName(),
                Key = fileName
            };

            await _s3Client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking file existence for {FileName}", fileName);
            return false;
        }
    }

    private void ValidateConfiguration()
    {
        var accountId = Environment.GetEnvironmentVariable("CLOUDFLARE_ACCOUNT_ID") ?? _config.AccountId;
        var accessKeyId = Environment.GetEnvironmentVariable("CLOUDFLARE_ACCESS_KEY_ID") ?? _config.AccessKeyId;
        var secretAccessKey = Environment.GetEnvironmentVariable("CLOUDFLARE_SECRET_ACCESS_KEY") ?? _config.SecretAccessKey;
        var bucketName = Environment.GetEnvironmentVariable("CLOUDFLARE_BUCKET_NAME") ?? _config.BucketName;
        
        if (string.IsNullOrEmpty(accountId))
            throw new InvalidOperationException("Cloudflare R2 AccountId is required (set CLOUDFLARE_ACCOUNT_ID or appsettings)");
        
        if (string.IsNullOrEmpty(accessKeyId))
            throw new InvalidOperationException("Cloudflare R2 AccessKeyId is required (set CLOUDFLARE_ACCESS_KEY_ID or appsettings)");
        
        if (string.IsNullOrEmpty(secretAccessKey))
            throw new InvalidOperationException("Cloudflare R2 SecretAccessKey is required (set CLOUDFLARE_SECRET_ACCESS_KEY or appsettings)");
        
        if (string.IsNullOrEmpty(bucketName))
            throw new InvalidOperationException("Cloudflare R2 BucketName is required (set CLOUDFLARE_BUCKET_NAME or appsettings)");
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        return $"{Guid.NewGuid()}{extension}";
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