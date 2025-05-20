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
    public class CaptureDetailRepository : GenericRepository<CaptureDetailsEntities>, ICaptureDetailRepository<CaptureDetailsEntities>
    {
        public CaptureDetailRepository(FloraFaunaGoDB context) : base(context) { }

        public async Task<Pagination<CaptureDetailsEntities>> GetAllCaptureDetail(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.None, int index = 0, int count = 15)
        {
            IQueryable<CaptureDetailsEntities> query = Set;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<CaptureDetailsEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<CaptureDetailsEntities>> GetCaptureDetailByCapture(string id,CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCapture, int index = 0, int count = 15)
        {
            IQueryable<CaptureDetailsEntities> query = Set;
            query = query.Where(captureDetail => captureDetail.CaptureId == id);
            /*query = query.OrderBy(captureDetail => captureDetail.CaptureId);*/

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<CaptureDetailsEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<CaptureDetailsEntities>> GetCaptureDetailByDate(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCaptureDate, int index = 0, int count = 15)
        {
            IQueryable<CaptureDetailsEntities> query = Set;
            query = query.OrderBy(captureDetail => captureDetail.DateCapture);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<CaptureDetailsEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<CaptureDetailsEntities>> GetCaptureDetailByLocation(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCaptureLocation, int index = 0, int count = 15)
        {
            IQueryable<CaptureDetailsEntities> query = Set;
            query = query.OrderBy(captureDetail => captureDetail.Localisation);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<CaptureDetailsEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }
    }
}
