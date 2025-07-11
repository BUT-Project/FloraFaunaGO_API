using FloraFauna_GO_Shared.Enums;

namespace FloraFauna_GO_Shared.Configuration;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";
    
    public FileStorageProvider DefaultProvider { get; set; } = FileStorageProvider.MinIO;
    public MinIOConfiguration MinIO { get; set; } = new();
    public CloudflareR2Configuration CloudflareR2 { get; set; } = new();
}

public class CloudflareR2Configuration
{
    public string AccountId { get; set; } = string.Empty;
    public string AccessKeyId { get; set; } = string.Empty;
    public string SecretAccessKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = "auto";
}