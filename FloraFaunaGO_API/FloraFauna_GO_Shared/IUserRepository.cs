using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Shared
{
    public interface IUserRepository<Tinput, Toutput> : IGenericRepository<Tinput, Toutput>
        where Toutput : class
        where Tinput : class
    {
        Task<Pagination<Toutput>> GetAllUser(UserOrderingCriteria criteria = UserOrderingCriteria.None,
            int index = 0, int count = 10);

        Task<Pagination<Toutput>> GetUserById(UserOrderingCriteria criteria = UserOrderingCriteria.Id,
            int index = 0, int count = 5);

        Task<Pagination<Toutput>> GetUserMail(UserOrderingCriteria criteria = UserOrderingCriteria.Mail,
            int index = 0, int count = 5);

        Task<Pagination<Toutput>> GetUserByMail(string mail,UserOrderingCriteria criteria = UserOrderingCriteria.Mail,
            int index = 0, int count = 5);

        Task<Pagination<Toutput>> GetUserBySuccessState(string id, UserOrderingCriteria criteria = UserOrderingCriteria.None,
            int index = 0, int count = 5);

        Task<Pagination<Toutput>> GetUserByCapture(string id, UserOrderingCriteria criteria = UserOrderingCriteria.None,
            int index = 0, int count = 5);
    }

    public interface IUserRepository<T> : IUserRepository<T, T>
        where T : class
    {

    }
}
