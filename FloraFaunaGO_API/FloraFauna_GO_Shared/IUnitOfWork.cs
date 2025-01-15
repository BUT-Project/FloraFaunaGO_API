using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared
{
    public interface IUnitOfWork<TEspeceInput, TEspeceOutput, TCaptureInput, TCaptureOutput, TUserInput, TUserOutput,
                                 TSuccessInput, TSuccessOutput, TSuccessStateInput, TSuccessStateOutput> : IDisposable
        where TEspeceInput : class
        where TCaptureInput : class
        where TUserInput : class
        where TUserOutput : class
        where TEspeceOutput : class
        where TCaptureOutput : class
        where TSuccessInput : class
        where TSuccessOutput : class
        where TSuccessStateInput : class
        where TSuccessStateOutput : class
    {
        IUserRepository<TUserInput, TUserOutput> UserRepository { get; }
        ICaptureRepository<TCaptureInput, TCaptureOutput> CaptureRepository { get; }
        IEspeceRepository<TEspeceInput, TEspeceOutput> EspeceRepository { get; }
        ISuccessRepository<TSuccessInput, TSuccessOutput> SuccessRepository { get; }
        ISuccessStateRepository<TSuccessStateInput, TSuccessStateOutput> SuccessStateRepository { get; }

        Task<bool> AddNewEspece(IEnumerable<TEspeceInput> espece);

        Task<bool> AddNewCapture(IEnumerable<TCaptureInput> capture, IEnumerable<TUserInput> user);


        Task<IEnumerable<object?>?> SaveChangesAsync();

        Task RejectChangesAsync();
    }

    public interface IUnitOfWork<TEspeceInput, TCaptureInput, TUserInput, TSuccessInput, TSuccessStateInput> : IDisposable
        where TEspeceInput : class
        where TCaptureInput : class
        where TUserInput : class
        where TSuccessInput : class
        where TSuccessStateInput : class
    {
        IUserRepository<TUserInput> UserRepository { get; }
        ICaptureRepository<TCaptureInput> CaptureRepository { get; }
        IEspeceRepository<TEspeceInput> EspeceRepository { get; }
        ISuccessRepository<TSuccessInput> SuccessRepository { get; }
        ISuccessStateRepository<TSuccessStateInput> SuccessStateRepository { get; }

        Task<bool> AddNewEspece(IEnumerable<TEspeceInput> espece);

        Task<bool> AddNewCapture(IEnumerable<TCaptureInput> capture, IEnumerable<TUserInput> user);


        Task<IEnumerable<object?>?> SaveChangesAsync();

        Task RejectChangesAsync();
    }
}
