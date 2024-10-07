using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared
{
    public interface IUnitOfWork<TEspeceInput, TEspeceOutput, TCaptureInput, TCaptureOutput, TUserInput, TUserOutput> : IDisposable
        where TEspeceInput : class
        where TCaptureInput : class
        where TUserInput : class
        where TUserOutput : class
        where TEspeceOutput : class
        where TCaptureOutput : class
    {
        IUserRepository<TUserInput, TUserOutput> UserRepository { get; }
        ICaptureRepository<TCaptureInput, TCaptureOutput> CaptureRepository { get; }
        IEspeceRepository<TEspeceInput, TEspeceOutput> EspeceRepository { get; }

        Task<bool> AddNewEspece(IEnumerable<TEspeceInput> espece);

        Task<bool> AddNewCapture(IEnumerable<TCaptureInput> capture, IEnumerable<TUserInput> user);


        Task<IEnumerable<object?>?> SaveChangesAsync();

        Task RejectChangesAsync();
    }

    public interface IUnitOfWork<TEspeceInput, TCaptureInput, TUserInput> : IDisposable
        where TEspeceInput : class
        where TCaptureInput : class
        where TUserInput : class
    {
        IUserRepository<TUserInput> UserRepository { get; }
        ICaptureRepository<TCaptureInput> CaptureRepository { get; }
        IEspeceRepository<TEspeceInput> EspeceRepository { get; }

        Task<bool> AddNewEspece(IEnumerable<TEspeceInput> espece);

        Task<bool> AddNewCapture(IEnumerable<TCaptureInput> capture, IEnumerable<TUserInput> user);


        Task<IEnumerable<object?>?> SaveChangesAsync();

        Task RejectChangesAsync();
    }
}
