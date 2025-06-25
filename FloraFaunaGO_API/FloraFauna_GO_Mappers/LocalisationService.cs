using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;

namespace FloraFauna_GO_Entities2Dto
{
    public class LocalisationService : ILocalisationRepository<LocalisationNormalDto, LocalisationNormalDto>
    {
        private ILocalisationRepository<LocalisationEntities> Repository { get; set; }
        public LocalisationService(ILocalisationRepository<LocalisationEntities> repository)
        {
            Repository = repository;
        }

        public async Task<bool> Delete(string id)
            => await Repository.Delete(id);

        public async Task<Pagination<LocalisationNormalDto>> GetAllLocalisation(int index = 0, int count = 15)
            => (await Repository.GetAllLocalisation(index, count)).ToPagingResponseDtos();

        public async Task<Pagination<LocalisationNormalDto>> GetLocalisationByCaptureDetail(string idCaptureDetail, int index = 0, int count = 15)
            => (await Repository.GetLocalisationByCaptureDetail(idCaptureDetail, index, count)).ToPagingResponseDtos();

        public async Task<Pagination<LocalisationNormalDto>> GetLocalisationByEspece(string idEspece, int index = 0, int count = 15)
            => (await Repository.GetLocalisationByEspece(idEspece, index, count)).ToPagingResponseDtos();

        public async Task<LocalisationNormalDto?> GetById(string id)
            => (await Repository.GetById(id))?.ToDto();

        public async Task<LocalisationNormalDto?> Insert(LocalisationNormalDto item)
            => (await Repository.Insert(item.ToEntities()))?.ToDto();

        public async Task<LocalisationNormalDto?> Update(string id, LocalisationNormalDto item)
            => (await Repository.Update(id, item.ToEntities()))?.ToDto();
    }
}
