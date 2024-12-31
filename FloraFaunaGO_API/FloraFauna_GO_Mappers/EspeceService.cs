using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Entities2Dto;

internal class EspeceService : IEspeceRepository<FullEspeceDto, FullEspeceDto>
{
    private IEspeceRepository<EspeceEntities> Repository { get; set; }

    public EspeceService(IEspeceRepository<EspeceEntities> repository)
    {
        Repository = repository;
    }
    public async Task<bool> Delete(string id) => await Repository.Delete(id);

    public async Task<Pagination<FullEspeceDto>> GetAllEspece(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.None, int index = 0, int count = 15) 
            => (await Repository.GetAllEspece(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullEspeceDto?> GetById(string id) 
        => (await Repository.GetById(id))?.ToDto();

    public async Task<Pagination<FullEspeceDto>> GetEspeceByFamile(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByFamille, int index = 0, int count = 15)
        => (await Repository.GetEspeceByFamile(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullEspeceDto>> GetEspeceByHabitat(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByHabitat, int index = 0, int count = 15)
        => (await Repository.GetEspeceByHabitat(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullEspeceDto>> GetEspeceByName(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByNom, int index = 0, int count = 15)
        => (await Repository.GetEspeceByName(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullEspeceDto>> GetEspeceByRegime(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByRegime, int index = 0, int count = 15)
        => (await Repository.GetEspeceByRegime(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullEspeceDto?> Insert(FullEspeceDto item)
        => (await Repository.Insert(item.ToEntites()))?.ToDto();

    public async Task<FullEspeceDto?> Update(string id, FullEspeceDto item)
        => (await Repository.Update(id, item.ToEntites()))?.ToDto();
}
