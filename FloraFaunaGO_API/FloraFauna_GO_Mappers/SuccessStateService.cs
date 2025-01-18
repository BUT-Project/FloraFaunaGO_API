using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities2Dto;

internal class SuccessStateService : ISuccessStateRepository<FullSuccessStateDto, FullSuccessStateDto>
{
    private ISuccessStateRepository<SuccesStateEntities> Repository { get; set; }
    public SuccessStateService(ISuccessStateRepository<SuccesStateEntities> repository)
    {
        Repository = repository;
    }
    public async Task<bool> Delete(string id) => await Repository.Delete(id);

    public async Task<Pagination<FullSuccessStateDto>> GetAllSuccessState(SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.None, int index = 0, int count = 10)
        => (await Repository.GetAllSuccessState(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullSuccessStateDto?> GetById(string id)
        => (await Repository.GetById(id))?.ToDto();

    public async Task<Pagination<FullSuccessStateDto>> GetSuccessStateBySuccess(SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.BySuccess, int index = 0, int count = 10)
        => (await Repository.GetSuccessStateBySuccess(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullSuccessStateDto>> GetSuccessStateByUser(SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.ByUser, int index = 0, int count = 10)
        => (await Repository.GetSuccessStateByUser(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullSuccessStateDto?> Insert(FullSuccessStateDto item)
        => (await Repository.Insert(item.ToEntities()))?.ToDto();

    public async Task<FullSuccessStateDto?> Update(string id, FullSuccessStateDto item)
        => (await Repository.Update(id, item.ToEntities()))?.ToDto();
}
