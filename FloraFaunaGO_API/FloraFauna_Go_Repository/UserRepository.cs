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
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<UtilisateurEntities>> GetUserById(UserOrderingCriteria criteria = UserOrderingCriteria.Id, int index = 0, int count = 5)
        {
            IQueryable<UtilisateurEntities> query = Set;

            query = query.OrderBy(user => user.Id);
            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<UtilisateurEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<UtilisateurEntities>> GetUserMail(UserOrderingCriteria criteria = UserOrderingCriteria.Mail, int index = 0, int count = 5)
        {
            IQueryable<UtilisateurEntities> query = Set;

            query = query.OrderBy(user => user.Mail);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<UtilisateurEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }
    }
}
