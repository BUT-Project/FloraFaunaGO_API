using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;

namespace FloraFauna_GO_Entities2Dto;

public static class Mappers
{
    internal static Mapper<UtilisateurNormalDto, UtilisateurEntities> UtilisateurMapper { get; } = new Mapper<UtilisateurNormalDto, UtilisateurEntities>();
    internal static Mapper<CaptureDetailNormalDto, CaptureDetailsEntities> CaptureDetailMapper { get; } = new Mapper<CaptureDetailNormalDto, CaptureDetailsEntities>();
    internal static Mapper<CaptureNormalDto, CaptureEntities> CaptureMapper { get; } = new Mapper<CaptureNormalDto, CaptureEntities>();
    internal static Mapper<EspeceNormalDto, EspeceEntities> EspeceMapper { get; } = new Mapper<EspeceNormalDto, EspeceEntities>();
    internal static Mapper<SuccessNormalDto, SuccesEntities> SuccessMapper { get; } = new Mapper<SuccessNormalDto, SuccesEntities>();
    internal static Mapper<SuccessStateNormalDto, SuccesStateEntities> SuccessStateMapper { get; } = new Mapper<SuccessStateNormalDto, SuccesStateEntities>();
    internal static Mapper<LocalisationNormalDto, LocalisationEntities> LocalisationMapper { get; } = new Mapper<LocalisationNormalDto, LocalisationEntities>();

    internal static Mapper<FullSuccessStateDto, SuccesStateEntities> ResponseSuccessStateMapper { get; } = new Mapper<FullSuccessStateDto, SuccesStateEntities>();
    internal static Mapper<FullUtilisateurDto, UtilisateurEntities> ResponseUtilisateurMapper { get; } = new Mapper<FullUtilisateurDto, UtilisateurEntities>();
    internal static Mapper<FullCaptureDetailDto, CaptureDetailsEntities> ResponseCaptureDetailMapper { get; } = new Mapper<FullCaptureDetailDto, CaptureDetailsEntities>();
    internal static Mapper<FullCaptureDto, CaptureEntities> ResponseCaptureMapper { get; } = new Mapper<FullCaptureDto, CaptureEntities>();
    internal static Mapper<FullEspeceDto, EspeceEntities> ResponseEspeceMapper { get; } = new Mapper<FullEspeceDto, EspeceEntities>();


    internal static void Reset()
    {
        UtilisateurMapper.Reset();
        CaptureDetailMapper.Reset();
        CaptureMapper.Reset();
        EspeceMapper.Reset();
        SuccessMapper.Reset();
        SuccessStateMapper.Reset();
        LocalisationMapper.Reset();

        ResponseSuccessStateMapper.Reset();
        ResponseUtilisateurMapper.Reset();
        ResponseCaptureDetailMapper.Reset();
        ResponseCaptureMapper.Reset();
        ResponseEspeceMapper.Reset();
    }
}
