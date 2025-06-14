using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;

namespace FloraFauna_GO_Entities2Dto;

public class CaptureDetailService : ICaptureDetailRepository<CaptureDetailNormalDto, FullCaptureDetailDto>
{
    private ICaptureDetailRepository<CaptureDetailsEntities> Repository { get; set; }
    public CaptureDetailService(ICaptureDetailRepository<CaptureDetailsEntities> repository)
    {
        Repository = repository;
    }
    public async Task<bool> Delete(string id) => await Repository.Delete(id);

    public async Task<Pagination<FullCaptureDetailDto>> GetAllCaptureDetail(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.None, int index = 0, int count = 15)
        => (await Repository.GetAllCaptureDetail(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullCaptureDetailDto?> GetById(string id)
        => (await Repository.GetById(id))?.ToResponseDto();

    public async Task<Pagination<FullCaptureDetailDto>> GetCaptureDetailByCapture(string id, CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCapture, int index = 0, int count = 15)
        => (await Repository.GetCaptureDetailByCapture(id, criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullCaptureDetailDto>> GetCaptureDetailByDate(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCaptureDate, int index = 0, int count = 15)
        => (await Repository.GetCaptureDetailByDate(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullCaptureDetailDto>> GetCaptureDetailByLocation(CaptureDetailOrderingCriteria criteria = CaptureDetailOrderingCriteria.ByCaptureLocation, int index = 0, int count = 15)
        => (await Repository.GetCaptureDetailByLocation(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullCaptureDetailDto?> Insert(CaptureDetailNormalDto item)
        => (await Repository.Insert(item.ToEntities()))?.ToResponseDto();

    public async Task<FullCaptureDetailDto?> Update(string id, CaptureDetailNormalDto item)
        => (await Repository.Update(id, item.ToEntities()))?.ToResponseDto();
}
