using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Shared
{
    public interface IEspeceRepository<Tinput, Toutput> : IGenericRepository<Tinput, Toutput>
        where Toutput : class
        where Tinput : class
    {
        Task<Pagination<Toutput>> GetAllEspece(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.None, 
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetEspeceByName(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByNom,
            int index=0, int count = 15);

        Task<Pagination<Toutput>> GetEspeceByFamile(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByFamille,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetEspeceByRegime(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByRegime,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetEspeceByClimat(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByClimat,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetEspeceByZone(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByZone,
            int index = 0, int count = 15);
    }

    public interface IEspeceRepository<T> : IEspeceRepository<T, T>
        where T : class
    {

    }
}
