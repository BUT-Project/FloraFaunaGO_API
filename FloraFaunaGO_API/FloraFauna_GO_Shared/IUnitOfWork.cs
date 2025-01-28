using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared
{
    public interface IUnitOfWork<TEspeceInput, TEspeceOutput, TCaptureInput, TCaptureOutput, TCaptureDetailInput, TCaptureDetailOutput,
                                 TUserInput, TUserOutput, TSuccessInput, TSuccessOutput, TSuccessStateInput, TSuccessStateOutput,
                                 TLocalisationInput, TLocalisationOutput> : IDisposable
        where TEspeceInput : class
        where TCaptureInput : class
        where TCaptureDetailInput : class
        where TUserInput : class
        where TSuccessInput : class
        where TSuccessStateInput : class
        where TLocalisationInput : class
        //
        where TEspeceOutput : class
        where TCaptureOutput : class
        where TCaptureDetailOutput : class
        where TUserOutput : class
        where TSuccessOutput : class
        where TSuccessStateOutput : class
        where TLocalisationOutput : class

    {
        IUserRepository<TUserInput, TUserOutput> UserRepository { get; }
        ICaptureRepository<TCaptureInput, TCaptureOutput> CaptureRepository { get; }
        ICaptureDetailRepository<TCaptureDetailInput, TCaptureDetailOutput> CaptureDetailRepository { get; }
        IEspeceRepository<TEspeceInput, TEspeceOutput> EspeceRepository { get; }
        ISuccessRepository<TSuccessInput, TSuccessOutput> SuccessRepository { get; }
        ISuccessStateRepository<TSuccessStateInput, TSuccessStateOutput> SuccessStateRepository { get; }
        ILocalisationRepository<TLocalisationInput, TLocalisationOutput> LocalisationRepository { get; }

        Task<bool> AddSuccesStateAsync(TSuccessStateInput successState, TUserInput user, TSuccessInput success);
        Task<bool> DeleteSuccesStateAsync(TSuccessStateInput successState, TUserInput user, TSuccessInput success);
        Task<bool> AddCaptureAsync(TCaptureInput capture, TUserInput user);
        Task<bool> DeleteCaptureAsync(TCaptureInput capture, TUserInput user, IEnumerable<TCaptureDetailInput> captureDetails);
        Task<bool> AddCaptureDetailAsync(TCaptureDetailInput captureDetail, TCaptureInput capture, TLocalisationInput localisation);
        Task<bool> DeleteCaptureDetailAsync(TCaptureDetailInput captureDetail, TCaptureInput capture, TLocalisationInput localisation);
        Task<bool> AddEspeceAsync(TEspeceInput espece, TLocalisationInput localisation);
        Task<bool> DeleteEspeceAsync(TEspeceInput espece, TLocalisationInput localisation);
        Task<bool> DeleteUser(TUserInput user, IEnumerable<TCaptureInput> captures, IEnumerable<TSuccessStateInput> successStates);

        Task<IEnumerable<object?>?> SaveChangesAsync();
        Task RejectChangesAsync();
    }

    public interface IUnitOfWork<TEspeceInput, TCaptureInput, TCaptureDetailInput, TUserInput, TSuccessInput, 
                                 TSuccessStateInput, TLocalisationInput> : IDisposable
        where TEspeceInput : class
        where TCaptureInput : class
        where TCaptureDetailInput : class
        where TUserInput : class
        where TSuccessInput : class
        where TSuccessStateInput : class
        where TLocalisationInput : class
    {
        IUserRepository<TUserInput> UserRepository { get; }
        ICaptureRepository<TCaptureInput> CaptureRepository { get; }
        ICaptureDetailRepository<TCaptureDetailInput> CaptureDetailRepository { get; }
        IEspeceRepository<TEspeceInput> EspeceRepository { get; }
        ISuccessRepository<TSuccessInput> SuccessRepository { get; }
        ISuccessStateRepository<TSuccessStateInput> SuccessStateRepository { get; }
        ILocalisationRepository<TLocalisationInput> LocalisationRepository { get; }

        Task<bool> AddSuccesStateAsync(TSuccessStateInput successState, TUserInput user, TSuccessInput success);
        Task<bool> DeleteSuccesStateAsync(TSuccessStateInput successState, TUserInput user, TSuccessInput success);
        Task<bool> AddCaptureAsync(TCaptureInput capture, TUserInput user);
        Task<bool> DeleteCaptureAsync(TCaptureInput capture, TUserInput user, IEnumerable<TCaptureDetailInput> captureDetails);
        Task<bool> AddCaptureDetailAsync(TCaptureDetailInput captureDetail, TCaptureInput capture, TLocalisationInput localisation);
        Task<bool> DeleteCaptureDetailAsync(TCaptureDetailInput captureDetail, TCaptureInput capture, TLocalisationInput localisation);
        Task<bool> AddEspeceAsync(TEspeceInput espece, TLocalisationInput localisation);
        Task<bool> DeleteEspeceAsync(TEspeceInput espece, TLocalisationInput localisation);
        Task<bool> DeleteUser(TUserInput user, IEnumerable<TCaptureInput> captures, IEnumerable<TSuccessStateInput> successStates);

        Task<IEnumerable<object?>?> SaveChangesAsync();
        Task RejectChangesAsync();
    }
}
