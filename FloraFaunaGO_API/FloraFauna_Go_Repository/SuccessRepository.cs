using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_Go_Repository
{
    public class SuccessRepository : GenericRepository<SuccesEntities>, ISuccessRepository<SuccesEntities>
    {
        public SuccessRepository(FloraFaunaGoDB context) : base(context) { }

        public async Task<Pagination<SuccesEntities>> GetAllSuccess(SuccessOrderingCreteria criteria = SuccessOrderingCreteria.None, int index = 0, int count = 10)
        {
            IQueryable<SuccesEntities> query = Set;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<SuccesEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<SuccesEntities>> GetSuccessByName(string name, SuccessOrderingCreteria criteria = SuccessOrderingCreteria.ByName, int index = 0, int count = 10)
        {
            IQueryable<SuccesEntities> query = Set;
            query = query.OrderBy(success => success.Nom);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();
            return new Pagination<SuccesEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<SuccesEntities>> GetSuccessBySuccessState(string id, SuccessOrderingCreteria criteria = SuccessOrderingCreteria.None, int index = 0, int count = 10)
        {
            IQueryable<SuccesEntities> query = Set;
            query = query.Where(success => success.SuccesStates.Any(sState => sState.Id == id));

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<SuccesEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }
    }
}
