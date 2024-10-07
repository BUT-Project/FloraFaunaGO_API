using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
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

        public Task<Pagination<UtilisateurEntities>> GetAllUser(UserOrderingCriteria criteria = UserOrderingCriteria.None, int index = 0, int count = 10)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<UtilisateurEntities>> GetUserById(UserOrderingCriteria criteria = UserOrderingCriteria.Id, int index = 0, int count = 5)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<UtilisateurEntities>> GetUserMail(UserOrderingCriteria criteria = UserOrderingCriteria.Mail, int index = 0, int count = 5)
        {
            throw new NotImplementedException();
        }
    }
}
