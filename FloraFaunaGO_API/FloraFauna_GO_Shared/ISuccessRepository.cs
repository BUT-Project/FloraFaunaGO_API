﻿using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Shared
{
    public interface ISuccessRepository<Tinput, Toutput> : IGenericRepository<Tinput, Toutput>
        where Toutput : class
        where Tinput : class
    {
        Task<Pagination<Toutput>> GetAllSuccess(SuccessOrderingCreteria criteria = SuccessOrderingCreteria.None,
            int index = 0, int count = 10);

        Task<Pagination<Toutput>> GetSuccessByName(string name, SuccessOrderingCreteria criteria = SuccessOrderingCreteria.ByName,
                int index = 0, int count = 10);

        Task<Pagination<Toutput>> GetSuccessBySuccessState(string id, SuccessOrderingCreteria criteria = SuccessOrderingCreteria.None,
                int index = 0, int count = 10);
    }

    public interface ISuccessRepository<T> : ISuccessRepository<T, T>
        where T : class
    {

    }

}
