using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_GO_Entities2Dto;

public class FloraFaunaService : IUnitOfWork<EspeceNormalDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto,CaptureDetailNormalDto,FullCaptureDetailDto,UtilisateurNormalDto, FullUtilisateurDto,SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto>
{
    
    private IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities, SuccesEntities, SuccesStateEntities, LocalisationEntities> DbUnitOfWork { get; set; }

    public FloraFaunaService(IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities,UtilisateurEntities, SuccesEntities, SuccesStateEntities, LocalisationEntities> dbUnitOfWork)
    {
        DbUnitOfWork = dbUnitOfWork;
        Mappers.Reset();
    }

    public FloraFaunaService(DbContextOptions<FloraFaunaGoDB> options)
        : this(new UnitOfWork(new FloraFaunaGoDB(options))) { }

    public IUserRepository<UtilisateurNormalDto, FullUtilisateurDto> UserRepository => new UserService(DbUnitOfWork.UserRepository);
    public ICaptureRepository<CaptureNormalDto, FullCaptureDto> CaptureRepository => new CaptureService(DbUnitOfWork.CaptureRepository);
    public IEspeceRepository<EspeceNormalDto, FullEspeceDto> EspeceRepository => new EspeceService(DbUnitOfWork.EspeceRepository);

    public ICaptureDetailRepository<CaptureDetailNormalDto, FullCaptureDetailDto> CaptureDetailRepository => new CaptureDetailService(DbUnitOfWork.CaptureDetailRepository);

    public ISuccessRepository<SuccessNormalDto, SuccessNormalDto> SuccessRepository => new SuccessService(DbUnitOfWork.SuccessRepository);

    public ISuccessStateRepository<SuccessStateNormalDto, FullSuccessStateDto> SuccessStateRepository => new SuccessStateService(DbUnitOfWork.SuccessStateRepository);

    public ILocalisationRepository<LocalisationNormalDto, LocalisationNormalDto> LocalisationRepository => throw new NotImplementedException();

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

    public async Task<bool> AddSuccesStateAsync(SuccessStateNormalDto successState, UtilisateurNormalDto user, SuccessNormalDto success)
    {
        bool result = await DbUnitOfWork.AddSuccesStateAsync(successState.ToEntities(), user.ToEntities(), success.ToEntities());
        return result;
    }

    public async Task<bool> DeleteSuccesStateAsync(SuccessStateNormalDto successState, UtilisateurNormalDto user, SuccessNormalDto success)
    {
        bool result = await DbUnitOfWork.DeleteSuccesStateAsync(successState.ToEntities(), user.ToEntities(), success.ToEntities());
        return result;
    }

    public async Task<bool> AddCaptureAsync(CaptureNormalDto capture, UtilisateurNormalDto user)
    {
       bool result = await DbUnitOfWork.AddCaptureAsync(capture.ToEntities(), user.ToEntities());
       return result;
    }

    public async Task<bool> DeleteCaptureAsync(CaptureNormalDto capture, UtilisateurNormalDto user, IEnumerable<CaptureDetailNormalDto> captureDetails)
    {
        bool result = await DbUnitOfWork.DeleteCaptureAsync(capture.ToEntities(), user.ToEntities(), captureDetails.Select(x => x.ToEntities()));
        return result;
    }

    public async Task<bool> AddCaptureDetailAsync(CaptureDetailNormalDto captureDetail, CaptureNormalDto capture, LocalisationNormalDto localisation)
    {
        bool result = await DbUnitOfWork.AddCaptureDetailAsync(captureDetail.ToEntities(), capture.ToEntities(), localisation.ToEntities());
        return result;
    }

    public async Task<bool> DeleteCaptureDetailAsync(CaptureDetailNormalDto captureDetail, CaptureNormalDto capture, LocalisationNormalDto localisation)
    {
        bool result = await DbUnitOfWork.DeleteCaptureDetailAsync(captureDetail.ToEntities(), capture.ToEntities(), localisation.ToEntities());
        return result;
    }

    public async Task<bool> DeleteUser(UtilisateurNormalDto user, IEnumerable<CaptureNormalDto> captures, IEnumerable<SuccessStateNormalDto> successStates)
    {
        bool result = await DbUnitOfWork.DeleteUser(user.ToEntities(), captures.Select(c => c.ToEntities()), successStates.Select(x => x.ToEntities())); 
        return result;
    }

    public Task<bool> AddEspeceAsync(EspeceNormalDto espece, LocalisationNormalDto localisation)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteEspeceAsync(EspeceNormalDto espece, LocalisationNormalDto localisation)
    {
        throw new NotImplementedException();
    }
}