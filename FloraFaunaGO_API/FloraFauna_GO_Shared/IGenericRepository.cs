using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared
{
    public interface IGenericRepository<TInput, TOutput>
        where TInput : class 
        where TOutput : class
    {
        Task<TOutput?> GetById(string id);

        Task<TOutput?> Insert(TInput item);

        Task<bool> Delete(string id);

        Task<TOutput?> Update(string id, TInput item);
    }

    public interface IGenericRepository<T> : IGenericRepository<T, T> 
        where T : class
    {

    }
}
