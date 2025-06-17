using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FloraFaunaGO_Services;

public class FileCleanupInterceptor : SaveChangesInterceptor
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<FileCleanupInterceptor> _logger;

    public FileCleanupInterceptor(IFileStorageService fileStorageService, ILogger<FileCleanupInterceptor> logger)
    {
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
        {
            await CleanupDeletedFilesAsync(eventData.Context);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context != null)
        {
            CleanupDeletedFilesAsync(eventData.Context).GetAwaiter().GetResult();
        }

        return base.SavingChanges(eventData, result);
    }

    private async Task CleanupDeletedFilesAsync(DbContext context)
    {
        var deletedEntities = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in deletedEntities)
        {
            try
            {
                await DeleteAssociatedFilesAsync(entry.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete files for entity {EntityType}", entry.Entity.GetType().Name);
                // Continue with other
            }
        }
    }

    private async Task DeleteAssociatedFilesAsync(object entity)
    {
        switch (entity)
        {
            case EspeceEntities espece:
                await DeleteFileIfExists(espece.ImageUrl);
                await DeleteFileIfExists(espece.Image3DUrl);
                break;

            case CaptureEntities capture:
                await DeleteFileIfExists(capture.PhotoUrl);
                break;

            case UtilisateurEntities user:
                await DeleteFileIfExists(user.ImageUrl);
                break;
        }
    }

    private async Task DeleteFileIfExists(string? fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return;

        try
        {
            var deleted = await _fileStorageService.DeleteAsync(fileUrl);
            if (deleted)
            {
                _logger.LogInformation("Successfully deleted file: {FileUrl}", fileUrl);
            }
            else
            {
                _logger.LogWarning("File not found or could not be deleted: {FileUrl}", fileUrl);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FileUrl}", fileUrl);
        }
    }
}