using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities2Dto;

internal class SuccessService : ISuccessRepository<SuccessNormalDto, SuccessNormalDto>
{
    private ISuccessRepository<SuccesEntities> Repository { get; set; }
    public SuccessService(ISuccessRepository<SuccesEntities> repository)
    {
        Repository = repository;
    }

    public async Task<bool> Delete(string id) => await Repository.Delete(id);

    public async Task<Pagination<SuccessNormalDto>> GetAllSuccess(SuccessOrderingCreteria criteria = SuccessOrderingCreteria.None, int index = 0, int count = 10)
        => (await Repository.GetAllSuccess(criteria, index, count)).ToPagingResponseDtos();

    public async Task<SuccessNormalDto?> GetById(string id)
        => (await Repository.GetById(id))?.ToDto();

    public async Task<Pagination<SuccessNormalDto>> GetSuccessByName(string name,SuccessOrderingCreteria criteria = SuccessOrderingCreteria.ByName, int index = 0, int count = 10)
        => (await Repository.GetSuccessByName(name,criteria, index, count)).ToPagingResponseDtos();

    public async Task<SuccessNormalDto?> Insert(SuccessNormalDto item)
        => (await Repository.Insert(item.ToEntities()))?.ToDto();

    public async Task<SuccessNormalDto?> Update(string id, SuccessNormalDto item)
        => (await Repository.Update(id, item.ToEntities()))?.ToDto();

    public async Task<Pagination<SuccessNormalDto>> GetSuccessBySuccessState(string id, SuccessOrderingCreteria criteria = SuccessOrderingCreteria.None, int index = 0, int count = 10)
        => (await Repository.GetSuccessBySuccessState(id, criteria, index, count)).ToPagingResponseDtos();
}
