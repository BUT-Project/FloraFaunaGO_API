using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_GO_Entities2Dto;

public class FloraFaunaService : IUnitOfWork<FullEspeceDto, FullEspeceDto, FullCaptureDto, FullCaptureDto,FullCaptureDetailDto,FullCaptureDetailDto,FullUtilisateurDto, FullUtilisateurDto,SuccessNormalDto, SuccessNormalDto, FullSuccessStateDto, FullSuccessStateDto>
{
    
    private IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities, SuccesEntities, SuccesStateEntities> DbUnitOfWork { get; set; }

    public FloraFaunaService(IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities,UtilisateurEntities, SuccesEntities, SuccesStateEntities> dbUnitOfWork)
    {
        DbUnitOfWork = dbUnitOfWork;
        Mappers.Reset();
    }

    public FloraFaunaService(DbContextOptions<FloraFaunaGoDB> options)
        : this(new UnitOfWork(new FloraFaunaGoDB(options))) { }

    public IUserRepository<FullUtilisateurDto, FullUtilisateurDto> UserRepository => new UserService(DbUnitOfWork.UserRepository);
    public ICaptureRepository<FullCaptureDto, FullCaptureDto> CaptureRepository => new CaptureService(DbUnitOfWork.CaptureRepository);
    public IEspeceRepository<FullEspeceDto, FullEspeceDto> EspeceRepository => new EspeceService(DbUnitOfWork.EspeceRepository);

    public ICaptureDetailRepository<FullCaptureDetailDto, FullCaptureDetailDto> CaptureDetailRepository => throw new NotImplementedException();

    public ISuccessRepository<SuccessNormalDto, SuccessNormalDto> SuccessRepository => throw new NotImplementedException();

    public ISuccessStateRepository<FullSuccessStateDto, FullSuccessStateDto> SuccessStateRepository => throw new NotImplementedException();

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

    public async Task<bool> AddSuccesStateAsync(FullSuccessStateDto successState, FullUtilisateurDto user, SuccessNormalDto success)
    {
        bool result = await DbUnitOfWork.AddSuccesStateAsync(successState.ToEntities(), user.ToEntities(), success.ToEntities());
        return result;
    }

    public async Task<bool> DeleteSuccesStateAsync(FullSuccessStateDto successState, FullUtilisateurDto user, SuccessNormalDto success)
    {
        bool result = await DbUnitOfWork.DeleteSuccesStateAsync(successState.ToEntities(), user.ToEntities(), success.ToEntities());
        return result;
    }

    public async Task<bool> AddCaptureAsync(FullCaptureDto capture, FullUtilisateurDto user)
    {
       bool result = await DbUnitOfWork.AddCaptureAsync(capture.ToEntities(), user.ToEntities());
       return result;
    }

    public async Task<bool> DeleteCaptureAsync(FullCaptureDto capture, FullUtilisateurDto user, IEnumerable<FullCaptureDetailDto> captureDetails)
    {
        bool result = await DbUnitOfWork.DeleteCaptureAsync(capture.ToEntities(), user.ToEntities(), captureDetails.Select(x => x.ToEntities()));
        return result;
    }

    public async Task<bool> AddCaptureDetailAsync(FullCaptureDetailDto captureDetail, FullCaptureDto capture)
    {
        bool result = await DbUnitOfWork.AddCaptureDetailAsync(captureDetail.ToEntities(), capture.ToEntities());
        return result;
    }

    public async Task<bool> DeleteCaptureDetailAsync(FullCaptureDetailDto captureDetail, FullCaptureDto capture)
    {
        bool result = await DbUnitOfWork.DeleteCaptureDetailAsync(captureDetail.ToEntities(), capture.ToEntities());
        return result;
    }

    public async Task<bool> DeleteUser(FullUtilisateurDto user, IEnumerable<FullCaptureDto> captures, IEnumerable<FullSuccessStateDto> successStates)
    {
        bool result = await DbUnitOfWork.DeleteUser(user.ToEntities(), captures.Select(c => c.ToEntities()), successStates.Select(x => x.ToEntities())); 
        return result;
    }
}