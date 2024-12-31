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
    class CaptureRepository : GenericRepository<CaptureEntities>, ICaptureRepository<CaptureEntities>
    {
        public CaptureRepository(FloraFaunaGoDB context) : base(context) { }

        public Task<Pagination<CaptureEntities>> GetAllCapture(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<CaptureEntities>> GetCaptureByDate(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByDateCapture, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<CaptureEntities>> GetCaptureByNumero(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByNumero, int index = 0, int count = 15)
        {
            throw new NotImplementedException();
        }
    }
}
