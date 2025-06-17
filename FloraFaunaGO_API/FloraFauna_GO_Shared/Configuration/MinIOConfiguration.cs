namespace FloraFauna_GO_Shared.Configuration;

public class MinIOConfiguration
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public bool UseSSL { get; set; } = false;
    public long MaxFileSize { get; set; } = 10485760; // 10MB
    public string[] AllowedExtensions { get; set; } = Array.Empty<string>();
}