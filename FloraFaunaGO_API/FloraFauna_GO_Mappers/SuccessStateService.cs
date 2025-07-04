﻿using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Entities2Dto;

public class SuccessStateService : ISuccessStateRepository<SuccessStateNormalDto, FullSuccessStateDto>
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
    {
        var result = await Repository.GetById(id);
        return result?.ToResponseDto();
    }
    //=> (await Repository.GetById(id))?.ToResponseDto();

    public async Task<Pagination<FullSuccessStateDto>> GetSuccessStateBySuccess(SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.BySuccess, int index = 0, int count = 10)
        => (await Repository.GetSuccessStateBySuccess(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullSuccessStateDto>> GetSuccessStateByUser(string id, SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.ByUser, int index = 0, int count = 10)
        => (await Repository.GetSuccessStateByUser(id, criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullSuccessStateDto?> Insert(SuccessStateNormalDto item)
        => (await Repository.Insert(item.ToEntities()))?.ToResponseDto();

    public async Task<FullSuccessStateDto?> Update(string id, SuccessStateNormalDto item)
        => (await Repository.Update(id, item.ToEntities()))?.ToResponseDto();

    public async Task<Pagination<FullSuccessStateDto>> GetSuccessStateByUser_Success(string idSuccess, string idUser, SuccessStateOrderingCreteria criteria = SuccessStateOrderingCreteria.ByUser, int index = 0, int count = 10)
        => (await Repository.GetSuccessStateByUser_Success(idSuccess, idUser, criteria, index, count)).ToPagingResponseDtos();
}
