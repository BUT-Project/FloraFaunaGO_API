using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Entities2Dto;

internal class CaptureService : ICaptureRepository<FullCaptureDto, FullCaptureDto>
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
        => (await Repository.GetById(id))?.ToDto();

    public async Task<Pagination<FullCaptureDto>> GetCaptureByDate(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByDateCapture, int index = 0, int count = 15)
        => (await Repository.GetCaptureByDate(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullCaptureDto>> GetCaptureByNumero(CaptureOrderingCriteria criteria = CaptureOrderingCriteria.ByNumero, int index = 0, int count = 15)
        => (await Repository.GetCaptureByNumero(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullCaptureDto?> Insert(FullCaptureDto item)
        => (await Repository.Insert(item.ToEntities()))?.ToDto();

    public async Task<FullCaptureDto?> Update(string id, FullCaptureDto item)
        => (await Repository.Update(id, item.ToEntities()))?.ToDto();
}
