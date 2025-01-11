using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Entities;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_GO_Entities2Dto;

public class FloraFaunaService : IUnitOfWork<FullEspeceDto, FullEspeceDto, FullCaptureDto, FullCaptureDto,FullUtilisateurDto, FullUtilisateurDto>
{
    
    private IUnitOfWork<EspeceEntities, CaptureEntities, UtilisateurEntities> DbUnitOfWork { get; set; }

    public FloraFaunaService(IUnitOfWork<EspeceEntities, CaptureEntities, UtilisateurEntities> dbUnitOfWork)
    {
        DbUnitOfWork = dbUnitOfWork;
        Mappers.Reset();
    }

    public FloraFaunaService(DbContextOptions<FloraFaunaGoDB> options) : this(new UnitOfWork(new FloraFaunaGoDB(options))){}

    public IUserRepository<FullUtilisateurDto, FullUtilisateurDto> UserRepository => new UserService(DbUnitOfWork.UserRepository);
    public ICaptureRepository<FullCaptureDto, FullCaptureDto> CaptureRepository => new CaptureService(DbUnitOfWork.CaptureRepository);
    public IEspeceRepository<FullEspeceDto, FullEspeceDto> EspeceRepository => new EspeceService(DbUnitOfWork.EspeceRepository);
    
    public async Task<bool> AddNewEspece(IEnumerable<FullEspeceDto> especes)
    {
        IEnumerable<EspeceEntities> especeEntities = especes.Select(dto => dto.ToEntites());
        bool result = await DbUnitOfWork.AddNewEspece(especeEntities);
        return result;
    }

    public async Task<bool> AddNewCapture(IEnumerable<FullCaptureDto> capture, IEnumerable<FullUtilisateurDto> user)
    {
        IEnumerable<CaptureEntities> captureEntities = capture.Select(dto => dto.ToEntities());
        IEnumerable<UtilisateurEntities> userEntities = user.Select(dto => dto.ToEntities());
        bool result = await DbUnitOfWork.AddNewCapture(captureEntities, userEntities);
        return result;
    }

    public async Task<IEnumerable<object?>?> SaveChangesAsync()
    {
            var result =await DbUnitOfWork.SaveChangesAsync();
            if (result == null) return null;

            static object? ToResponseDto(Object? obj)
            {
                if (obj is CaptureEntities) return (obj as CaptureEntities)?.ToDto();
                if (obj is UtilisateurEntities) return (obj as UtilisateurEntities)?.ToDto();
                if (obj is EspeceEntities) return (obj as EspeceEntities)?.ToDto();
                return null;
            }
            return result.Select(ToResponseDto);
    }

    public async Task RejectChangesAsync()
    {
        await DbUnitOfWork.RejectChangesAsync();
    }
    
    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed && disposing)
        {
            DbUnitOfWork.Dispose();
        }
        this.disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}