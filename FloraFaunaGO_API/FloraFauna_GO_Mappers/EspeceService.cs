using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Entities2Dto;

public class EspeceService : IEspeceRepository<FullEspeceDto, FullEspeceDto>
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
        => (await Repository.GetById(id))?.ToResponseDto();

    public async Task<Pagination<FullEspeceDto>> GetEspeceByFamile(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByFamille, int index = 0, int count = 15)
        => (await Repository.GetEspeceByFamile(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullEspeceDto>> GetEspeceByName(string name,EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByNom, int index = 0, int count = 15)
        => (await Repository.GetEspeceByName(name,criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullEspeceDto>> GetEspeceByRegime(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByRegime, int index = 0, int count = 15)
        => (await Repository.GetEspeceByRegime(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullEspeceDto?> Insert(FullEspeceDto item)
        => (await Repository.Insert(item.ToEntities()))?.ToResponseDto();

    public async Task<FullEspeceDto?> Update(string id, FullEspeceDto item)
        => (await Repository.Update(id, item.ToEntities()))?.ToResponseDto();

    public Task<Pagination<FullEspeceDto>> GetEspeceByClimat(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByClimat, int index = 0, int count = 15)
    {
        throw new NotImplementedException();
    }

    public Task<Pagination<FullEspeceDto>> GetEspeceByZone(EspeceOrderingCriteria criteria = EspeceOrderingCriteria.ByZone, int index = 0, int count = 15)
    {
        throw new NotImplementedException();
    }

    public Task<Pagination<FullEspeceDto>> GetEspeceByProperty(string id,string property, EspeceOrderingCriteria criteria = EspeceOrderingCriteria.None, int index = 0, int count = 15)
        => Repository.GetEspeceByProperty(id, property, criteria, index, count).ContinueWith(t => t.Result.ToPagingResponseDtos());
}
