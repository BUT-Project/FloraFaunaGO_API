using FloraFauna_GO_Shared.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared
{
    public interface ISuccessRepository<Tinput, Toutput> : IGenericRepository<Tinput, Toutput>
        where Toutput : class
        where Tinput : class
    {
        Task<Pagination<Toutput>> GetAllSuccess(SuccessOrderingCreteria criteria = SuccessOrderingCreteria.None,
            int index = 0, int count = 10);

        Task<Pagination<Toutput>> GetSuccessByName(SuccessOrderingCreteria criteria = SuccessOrderingCreteria.ByName,
                int index = 0, int count = 10);
    }

    public interface ISuccessRepository<T> : IUserRepository<T, T>
        where T : class
    {

    }
    
}
