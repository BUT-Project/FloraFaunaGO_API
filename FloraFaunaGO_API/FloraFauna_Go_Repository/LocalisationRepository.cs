using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_Go_Repository
{
    public class LocalisationRepository : GenericRepository<LocalisationEntities>, ILocalisationRepository<LocalisationEntities>
    {
        public LocalisationRepository(FloraFaunaGoDB context) : base(context) { }

        public async Task<Pagination<LocalisationEntities>> GetAllLocalisation(int index = 0, int count = 15)
        {
            IQueryable<LocalisationEntities> query = Set;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();

            return new Pagination<LocalisationEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<LocalisationEntities>> GetLocalisationByCaptureDetail(string idCaptureDetail, int index = 0, int count = 15)
        {
            IQueryable<LocalisationEntities> query = Set;
            query = query.OrderBy(success => success.CapturesDetail.CaptureId == idCaptureDetail);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();
            return new Pagination<LocalisationEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }

        public async Task<Pagination<LocalisationEntities>> GetLocalisationByEspece(string idEspece, int index = 0, int count = 15)
        {
            IQueryable<LocalisationEntities> query = Set;
            query = query.Where(success => success.EspeceLocalisation.Any(espece => espece.EspeceId == idEspece));
            /*query = query.OrderBy(success => success.EspeceLocalisation.FirstOrDefault(espece => espece.EspeceId == idEspece));*/

            var totalCount = await query.CountAsync();
            var items = await query.Skip(index * count).Take(count).ToListAsync();
            return new Pagination<LocalisationEntities>()
            {
                Total = totalCount,
                Index = index,
                Count = count,
                Items = items
            };
        }
    }
}
