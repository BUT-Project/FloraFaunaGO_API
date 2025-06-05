using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_Go_Repository
{
    public class UserRepository : GenericRepository<UtilisateurEntities>, IUserRepository<UtilisateurEntities>
    {
        public UserRepository(FloraFaunaGoDB context) : base(context) { }

        public async Task<Pagination<UtilisateurEntities>> GetAllUser(UserOrderingCriteria criteria = UserOrderingCriteria.None, int index = 0, int count = 10)
        {
            IQueryable<UtilisateurEntities> query = Set;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<UtilisateurEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public Task<Pagination<UtilisateurEntities>> GetUserByCapture(string id, UserOrderingCriteria criteria = UserOrderingCriteria.None, int index = 0, int count = 5)
        {
            IQueryable<UtilisateurEntities> query = Set;

            query = query.Where(user => user.Captures.Any(capture => capture.Id == id));

            var totalCount = query.Count();
            var items = query.Skip(index * count).Take(count).ToList();

            return Task.FromResult(new Pagination<UtilisateurEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            });
        }

        public async Task<Pagination<UtilisateurEntities>> GetUserById(UserOrderingCriteria criteria = UserOrderingCriteria.Id, int index = 0, int count = 5)
        {
            IQueryable<UtilisateurEntities> query = Set;

            query = query.OrderBy(user => user.Id);
            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<UtilisateurEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<UtilisateurEntities>> GetUserByMail(string mail, UserOrderingCriteria criteria = UserOrderingCriteria.Mail, int index = 0, int count = 5)
        {
            IQueryable<UtilisateurEntities> query = Set;

            query = query.Where(user => user.Email == mail);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<UtilisateurEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<UtilisateurEntities>> GetUserBySuccessState(string id, UserOrderingCriteria criteria = UserOrderingCriteria.None, int index = 0, int count = 5)
        {
            IQueryable<UtilisateurEntities> query = Set;

            query = query.Where(user => user.SuccesState != null && user.SuccesState.Any(sState => sState.Id == id));

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<UtilisateurEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<UtilisateurEntities>> GetUserMail(UserOrderingCriteria criteria = UserOrderingCriteria.Mail, int index = 0, int count = 5)
        {
            IQueryable<UtilisateurEntities> query = Set;

            query = query.OrderBy(user => user.Email);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<UtilisateurEntities>
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }
    }
}
