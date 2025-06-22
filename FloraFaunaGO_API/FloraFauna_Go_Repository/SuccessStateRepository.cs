using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_Go_Repository
{
    public class SuccessStateRepository : GenericRepository<SuccesStateEntities>, ISuccessStateRepository<SuccesStateEntities>
    {
        public SuccessStateRepository(FloraFaunaGoDB context) : base(context) { }

        public async Task<Pagination<SuccesStateEntities>> GetAllSuccessState(SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.None, int index = 0, int count = 10)
        {
            IQueryable<SuccesStateEntities> query = Set;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<SuccesStateEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<SuccesStateEntities>> GetSuccessStateBySuccess(SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.BySuccess, int index = 0, int count = 10)
        {
            IQueryable<SuccesStateEntities> query = Set;
            query = query.OrderBy(success => success.SuccesEntitiesId);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<SuccesStateEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<SuccesStateEntities>> GetSuccessStateByUser(string id, SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.ByUser, int index = 0, int count = 10)
        {
            IQueryable<SuccesStateEntities> query = Set;
            query = query.Where(success => success.UtilisateurId == id);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<SuccesStateEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public Task<Pagination<SuccesStateEntities>> GetSuccessStateByUser_Success(string idSuccess, string idUser, SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.ByUser, int index = 0, int count = 10)
        {
            IQueryable<SuccesStateEntities> query = Set;
            query = query.Where(success => success.UtilisateurId == idUser && success.SuccesEntitiesId == idSuccess);

            var totalCount = query.Count();
            var items = query.Skip(index * count).Take(count).ToListAsync();

            return Task.FromResult(new Pagination<SuccesStateEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items.Result
            });
        }
    }
}
