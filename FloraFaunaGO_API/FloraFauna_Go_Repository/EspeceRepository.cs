using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.EntityFrameworkCore;

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
            [EspeceOrderingCriteria.ByClimat] = query => query.OrderBy(espece => espece.Climat),
            [EspeceOrderingCriteria.ByZone] = query => query.OrderBy(espece => espece.Zone)
        };

        public EspeceRepository(FloraFaunaGoDB context) : base(context) { }


        public async Task<Pagination<EspeceEntities>> GetAllEspece(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.None, int index = 0, int count = 15)
        {
            IQueryable<EspeceEntities> query = Set;

            switch (criteria)
            {
                case EspeceOrderingCriteria.ByFamille:
                    query = query.OrderBy(espece => espece.Famille);
                    break;
                case EspeceOrderingCriteria.ByRegime:
                    query = query.OrderByDescending(espece => espece.Regime);
                    break;
                case EspeceOrderingCriteria.ByNom:
                    query = query.OrderBy(espece => espece.Nom);
                    break;
                case EspeceOrderingCriteria.ByClimat:
                    query = query.OrderBy(espece => espece.Climat);
                    break;
                case EspeceOrderingCriteria.ByZone:
                    query = query.OrderBy(espece => espece.Zone);
                    break;
                default:
                    break;
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<EspeceEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByZone(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByZone, int index = 0, int count = 15)
        {
            IQueryable<EspeceEntities> query = Set;
            query = query.OrderBy(espece => espece.Zone);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<EspeceEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByClimat(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByClimat, int index = 0, int count = 15)
        {
            IQueryable<EspeceEntities> query = Set;
            query = query.OrderBy(espece => espece.Climat);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<EspeceEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
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
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByName(string name, EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByNom, int index = 0, int count = 15)
        {
            IQueryable<EspeceEntities> query = Set;

            query = query.Where(espece => espece.Nom == name);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<EspeceEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByRegime(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByRegime, int index = 0, int count = 15)
        {
            IQueryable<EspeceEntities> query = Set;

            query = query.OrderByDescending(espece => espece.Regime);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<EspeceEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<EspeceEntities>> GetEspeceByProperty(string id, string property, EspeceOrderingCriteria criteria = EspeceOrderingCriteria.None, int index = 0, int count = 15)
        {
            IQueryable<EspeceEntities> query = Set;

            switch (criteria)
            {
                case EspeceOrderingCriteria.ByFamille:
                    query = query.Where(espece => espece.Famille == property);
                    break;
                case EspeceOrderingCriteria.ByRegime:
                    query = query.Where(espece => espece.Regime == property);
                    break;
                case EspeceOrderingCriteria.ByNom:
                    query = query.Where(espece => espece.Nom == property);
                    break;
                case EspeceOrderingCriteria.ByClimat:
                    query = query.Where(espece => espece.Climat == property);
                    break;
                case EspeceOrderingCriteria.ByZone:
                    query = query.Where(espece => espece.Zone == property);
                    break;
                default:
                    break;
            }

            query = query.Where(espece => espece.Id != id);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();


            return new Pagination<EspeceEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }
    }
}
