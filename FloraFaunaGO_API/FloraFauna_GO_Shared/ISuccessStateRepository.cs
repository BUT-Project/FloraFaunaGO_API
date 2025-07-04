﻿using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Shared
{
    public interface ISuccessStateRepository<Tinput, Toutput> : IGenericRepository<Tinput, Toutput>
        where Toutput : class
        where Tinput : class
    {
        Task<Pagination<Toutput>> GetAllSuccessState(SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.None,
            int index = 0, int count = 10);

        Task<Pagination<Toutput>> GetSuccessStateBySuccess(SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.BySuccess,
            int index = 0, int count = 10);

        Task<Pagination<Toutput>> GetSuccessStateByUser(string id, SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.ByUser,
            int index = 0, int count = 10);

        Task<Pagination<Toutput>> GetSuccessStateByUser_Success(string idSuccess, string idUser, SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.ByUser,
            int index = 0, int count = 10);
    }

    public interface ISuccessStateRepository<T> : ISuccessStateRepository<T, T>
        where T : class
    {

    }
}
