using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared.Criteria;

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

        public EspeceRepository(FloraFaunaGoDB context) : base(context) { }


        public async Task<Pagination<EspeceEntities>> GetAllEspece(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.None, int index = 0, int count = 15)
        {
            IQueryable<EspeceEntities> query = Set;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<EspeceEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByFamile(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByFamille, int index = 0, int count = 15)
        {
            IQueryable<EspeceEntities> query = Set;

            query = query.OrderBy(espece => espece.Famille);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<EspeceEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByHabitat(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByHabitat, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByName(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByNom, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByRegime(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByRegime, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }
    }
}
