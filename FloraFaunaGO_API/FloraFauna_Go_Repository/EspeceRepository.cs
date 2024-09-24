using FloraFauna_GO_Shared;
using FloraFaunaGO_Modele;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_Go_Repository
{
    public class EspeceRepository : GenericRepository<EspeceEntities>, IEspeceRepository<EspeceEntities>
    {
        protected readonly static Dictionary<EspeceOrderingCriteria, Func<IQueryable<EspeceEntities>, IQueryable<EspeceEntities>>> orderingFactory
        = new Dictionary<EspeceOrderingCriteria, Func<IQueryable<EspeceEntities>, IQueryable<EspeceEntities>>>()
        {
            [EspeceOrderingCriteria.None] = query => query,
            [EspeceOrderingCriteria.ByFamille] = query => query.OrderBy(espece => espece.Famille),
            [EspeceOrderingCriteria.ByRegime] = query => query.OrderByDescending(espece => espece.Regime),
            [EspeceOrderingCriteria.ByNom] = query => query.OrderBy(espece => espece.Nom),
        };

        // public EspeceRepository(DbContext context) : base(context) { }


        public Task<Pagination<EspeceEntities>> GetAllEspece(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.None, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<EspeceEntities>> GetEspeceByFamile(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByFamille, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<EspeceEntities>> GetEspeceByHabitat(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByHabitat, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<EspeceEntities>> GetEspeceByName(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByNom, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<EspeceEntities>> GetEspeceByRegime(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByRegime, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }
    }
}
