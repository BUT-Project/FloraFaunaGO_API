using FloraFauna_GO_Shared.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared
{
    public interface ICaptureDetailRepository<Tinput, Toutput> : IGenericRepository<Tinput, Toutput>
        where Tinput : class
        where Toutput : class
    {
        Task<Pagination<Toutput>> GetAllCaptureDetail(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.None,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetCaptureDetailByCapture(string id,CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCapture,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetCaptureDetailByLocation(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCaptureLocation,
            int index = 0, int count = 15);

        Task<Pagination<Toutput>> GetCaptureDetailByDate(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCaptureDate,
            int index = 0, int count = 15);
    }

    public interface ICaptureDetailRepository<T> : ICaptureDetailRepository<T, T>
        where T : class
    {

    }
}
