using FloraFauna_GO_Shared.Enums;

namespace FloraFauna_GO_Shared.Interfaces;

public interface IFileStorageFactory
{
    IFileStorageService Create(FileStorageProvider provider);
    IFileStorageService Create();
}