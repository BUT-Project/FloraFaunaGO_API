﻿using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Shared
{
    public interface ICaptureRepository<Tinput, Toutput> : IGenericRepository<Tinput, Toutput>
        where Tinput : class
        where Toutput : class
    {
        Task<Pagination<Toutput>> GetAllCapture(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetCaptureByNumero(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByNumero,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetCaptureByUser(string id, CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByUser,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetCaptureByCaptureDetail(string id, CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetCaptureByEspece(string id, CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None,
            int index = 0, int count = 15);
    }

    public interface ICaptureRepository<T> : ICaptureRepository<T, T>
        where T : class
    {

    }
}
