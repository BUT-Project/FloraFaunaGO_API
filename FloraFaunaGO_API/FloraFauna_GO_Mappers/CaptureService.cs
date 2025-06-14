using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Entities2Dto;

public class CaptureService : ICaptureRepository<CaptureNormalDto, FullCaptureDto>
{
    private ICaptureRepository<CaptureEntities> Repository { get; set; }

    public CaptureService(ICaptureRepository<CaptureEntities> repository)
    {
        Repository = repository;
    }

    public async Task<bool> Delete(string id) => await Repository.Delete(id);

    public async Task<Pagination<FullCaptureDto>> GetAllCapture(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None, int index = 0, int count = 15)
        => (await Repository.GetAllCapture(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullCaptureDto?> GetById(string id)
        => (await Repository.GetById(id))?.ToResponseDto();

    public async Task<Pagination<FullCaptureDto>> GetCaptureByNumero(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByNumero, int index = 0, int count = 15)
        => (await Repository.GetCaptureByNumero(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullCaptureDto?> Insert(CaptureNormalDto item)
        => (await Repository.Insert(item.ToEntities()))?.ToResponseDto();

    public async Task<FullCaptureDto?> Update(string id, CaptureNormalDto item)
        => (await Repository.Update(id, item.ToEntities()))?.ToResponseDto();

    public async Task<Pagination<FullCaptureDto>> GetCaptureByUser(string id, CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByUser, int index = 0, int count = 15)
        => (await Repository.GetCaptureByUser(id, criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullCaptureDto>> GetCaptureByCaptureDetail(string id, CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None, int index = 0, int count = 15)
        => (await Repository.GetCaptureByCaptureDetail(id, criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullCaptureDto>> GetCaptureByEspece(string id, CaptureOrderingCriteria criteria = CaptureOrderingCriteria.None, int index = 0, int count = 15)
        => (await Repository.GetCaptureByEspece(id, criteria, index, count)).ToPagingResponseDtos();
}
