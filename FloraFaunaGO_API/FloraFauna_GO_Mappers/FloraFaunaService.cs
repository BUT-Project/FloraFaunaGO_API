using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_GO_Entities2Dto;

public class FloraFaunaService : IUnitOfWork<FullEspeceDto, FullEspeceDto, CaptureNormalDto, FullCaptureDto, CaptureDetailNormalDto, FullCaptureDetailDto, UtilisateurNormalDto, FullUtilisateurDto, SuccessNormalDto, SuccessNormalDto, SuccessStateNormalDto, FullSuccessStateDto, LocalisationNormalDto, LocalisationNormalDto>
{

    private IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities, SuccesEntities, SuccesStateEntities, LocalisationEntities> DbUnitOfWork { get; set; }

    public FloraFaunaService(IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities, SuccesEntities, SuccesStateEntities, LocalisationEntities> dbUnitOfWork)
    {
        DbUnitOfWork = dbUnitOfWork;
        if (DbUnitOfWork == null)
        {
            Console.WriteLine("it's null");
        }

        Mappers.Reset();
    }

    public FloraFaunaService(DbContextOptions<FloraFaunaGoDB> options)
        : this(new UnitOfWork(new FloraFaunaGoDB(options))) { }

    public IUserRepository<UtilisateurNormalDto, FullUtilisateurDto> UserRepository => new UserService(DbUnitOfWork.UserRepository);
    public ICaptureRepository<CaptureNormalDto, FullCaptureDto> CaptureRepository => new CaptureService(DbUnitOfWork.CaptureRepository);
    public IEspeceRepository<FullEspeceDto, FullEspeceDto> EspeceRepository => new EspeceService(DbUnitOfWork.EspeceRepository);

    public ICaptureDetailRepository<CaptureDetailNormalDto, FullCaptureDetailDto> CaptureDetailRepository => new CaptureDetailService(DbUnitOfWork.CaptureDetailRepository);

    public ISuccessRepository<SuccessNormalDto, SuccessNormalDto> SuccessRepository => new SuccessService(DbUnitOfWork.SuccessRepository);

    public ISuccessStateRepository<SuccessStateNormalDto, FullSuccessStateDto> SuccessStateRepository => new SuccessStateService(DbUnitOfWork.SuccessStateRepository);

    public ILocalisationRepository<LocalisationNormalDto, LocalisationNormalDto> LocalisationRepository => new LocalisationService(DbUnitOfWork.LocalisationRepository);

    public async Task<IEnumerable<object?>?> SaveChangesAsync()
    {
        var result = await DbUnitOfWork.SaveChangesAsync();
        if (result == null) return null;

        static object? ToResponseDto(Object? obj)
        {
            if (obj is CaptureEntities) return (obj as CaptureEntities)?.ToDto();
            if (obj is UtilisateurEntities) return (obj as UtilisateurEntities)?.ToDto();
            if (obj is EspeceEntities) return (obj as EspeceEntities)?.ToDto();
            if (obj is SuccesEntities) return (obj as SuccesEntities)?.ToDto();
            if (obj is CaptureDetailsEntities) return (obj as CaptureDetailsEntities)?.ToDto();
            if (obj is LocalisationEntities) return (obj as LocalisationEntities)?.ToDto();
            if (obj is SuccesStateEntities) return (obj as SuccesStateEntities)?.ToDto();
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
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    public async Task<bool> AddSuccesStateAsync(SuccessStateNormalDto successState, UtilisateurNormalDto user, SuccessNormalDto success)
    {
        bool result = await DbUnitOfWork.AddSuccesStateAsync(successState.ToEntities(), user.ToEntities(user.Id), success.ToEntities(success.Id));
        return result;
    }

    public async Task<bool> DeleteSuccesStateAsync(SuccessStateNormalDto successState, UtilisateurNormalDto user, SuccessNormalDto success)
    {
        bool result = await DbUnitOfWork.DeleteSuccesStateAsync(successState.ToEntities(successState.Id), user.ToEntities(user.Id), success.ToEntities(success.Id));
        return result;
    }

    public async Task<bool> AddCaptureAsync(CaptureNormalDto capture, UtilisateurNormalDto user)
    {
        bool result = await DbUnitOfWork.AddCaptureAsync(capture.ToEntities(), user.ToEntities(user.Id));
        return result;
    }

    public async Task<bool> DeleteCaptureAsync(CaptureNormalDto capture, UtilisateurNormalDto user, IEnumerable<CaptureDetailNormalDto> captureDetails)
    {
        bool result = await DbUnitOfWork.DeleteCaptureAsync(capture.ToEntities(capture.Id), user.ToEntities(user.Id), captureDetails.Select(x => x.ToEntities(x.Id)));
        return result;
    }

    public async Task<bool> AddCaptureDetailAsync(CaptureDetailNormalDto captureDetail, CaptureNormalDto capture, LocalisationNormalDto localisation)
    {
        bool result = await DbUnitOfWork.AddCaptureDetailAsync(captureDetail.ToEntities(), capture.ToEntities(capture.Id), localisation.ToEntities());
        return result;
    }

    public async Task<bool> DeleteCaptureDetailAsync(CaptureDetailNormalDto captureDetail, CaptureNormalDto capture, LocalisationNormalDto localisation)
    {
        bool result = await DbUnitOfWork.DeleteCaptureDetailAsync(captureDetail.ToEntities(captureDetail.Id), capture.ToEntities(capture.Id), localisation.ToEntities(localisation.Id));
        return result;
    }

    public async Task<bool> DeleteUser(UtilisateurNormalDto user, IEnumerable<CaptureNormalDto> captures, IEnumerable<SuccessStateNormalDto> successStates)
    {
        var captureDto = captures.Select(c => c.ToEntities(c.Id));
        foreach (var capture in captureDto)
        {
            var captureDetails = await CaptureDetailRepository.GetCaptureDetailByCapture(capture.Id);
            var localisations = captureDetails.Items.Select(cd => cd.localisationNormalDtos);
        }
        bool result = await DbUnitOfWork.DeleteUser(user.ToEntities(user.Id), captureDto, successStates.Select(x => x.ToEntities(x.Id)));
        return result;
    }

    public async Task<bool> AddEspeceAsync(FullEspeceDto espece, IEnumerable<LocalisationNormalDto> localisations)
    {
        bool result = await DbUnitOfWork.AddEspeceAsync(espece.ToEntities(), localisations.Select(l => l.ToEntities()));
        return result;
    }

    public async Task<bool> DeleteEspeceAsync(FullEspeceDto espece, IEnumerable<LocalisationNormalDto> localisations)
    {
        bool result = await DbUnitOfWork.DeleteEspeceAsync(espece.ToEntities(espece.Id), localisations.Select(l => l.ToEntities(l.Id)));
        return result;
    }

    public async Task<bool> AddSuccess(SuccessNormalDto success)
    {
        bool result = await DbUnitOfWork.AddSuccess(success.ToEntities());
        return result;
    }
}