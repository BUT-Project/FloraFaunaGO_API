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

        public async Task<Pagination<CaptureEntities>> GetCaptureByCaptureDetail(string id, CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None, int index = 0, int count = 15)
        {
            IQueryable<CaptureEntities> query = Set;

            query = query.Where(capture => capture.CaptureDetails.Any(detail => detail.Id == id));
            /*query = query.OrderBy(capture => capture.UtilisateurId);*/

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

        public async Task<Pagination<CaptureEntities>> GetCaptureByUser(string id, CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByUser, int index = 0, int count = 15)
        {
            IQueryable<CaptureEntities> query = Set;

            query = query.Where(capture => capture.UtilisateurId == id);
            /*query = query.OrderBy(capture => capture.UtilisateurId);*/

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
