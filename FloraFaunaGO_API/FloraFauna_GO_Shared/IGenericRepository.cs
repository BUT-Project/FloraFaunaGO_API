using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared
{
    public interface IGenericRepository<TInput, TOutput>
        where TInput : class where TOutput : class
    {
        Task<TOutput?> GetById(object id);

        Task<TOutput?> Insert(TInput item);

        Task<bool> Delete(object id);

        Task<TOutput?> Update(object id, TInput item);
    }

    public interface IGenericRepository<T> : IGenericRepository<T, T> 
        where T : class
    {

    }
}
