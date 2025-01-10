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
    public class CaptureRepository : GenericRepository<CaptureEntities>, ICaptureRepository<CaptureEntities>
    {
        public CaptureRepository(FloraFaunaGoDB context) : base(context) { }

        public async Task<Pagination<CaptureEntities>> GetAllCapture(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None, int index = 0, int count = 15)
        {
            IQueryable<CaptureEntities> query = Set;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<CaptureEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<CaptureEntities>> GetCaptureByDate(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByDateCapture, int index = 0, int count = 15)
        {
            IQueryable<CaptureEntities> query = Set;

            // a voir ça ne marche pas
            // Je ne sais pas encore comment faire !
            //query = query.OrderBy(capture => capture.CaptureDetails);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<CaptureEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }

        public async Task<Pagination<CaptureEntities>> GetCaptureByNumero(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByNumero, int index = 0, int count = 15)
        {
            IQueryable<CaptureEntities> query = Set;

            query = query.OrderBy(capture => capture.Numero);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<CaptureEntities>
            {
                TotalCount = totalCount,
                PageIndex = index,
                CountPerPage = count,
                Items = items
            };
        }
    }
}
