using FloraFauna_GO_Shared.Configuration;
using FloraFauna_GO_Shared.Enums;
using FloraFauna_GO_Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FloraFaunaGO_Services;

public class FileStorageFactory : IFileStorageFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly FileStorageOptions _options;

    public FileStorageFactory(IServiceProvider serviceProvider, IOptions<FileStorageOptions> options)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public IFileStorageService Create(FileStorageProvider provider)
    {
        return provider switch
        {
            FileStorageProvider.MinIO => _serviceProvider.GetRequiredService<MinIoFileStorageService>(),
            FileStorageProvider.Cloudflare => _serviceProvider.GetRequiredService<CloudflareR2FileStorageService>(),
            _ => throw new NotSupportedException($"File storage provider '{provider}' is not supported")
        };
    }

    public IFileStorageService Create()
    {
        return Create(_options.DefaultProvider);
    }
}