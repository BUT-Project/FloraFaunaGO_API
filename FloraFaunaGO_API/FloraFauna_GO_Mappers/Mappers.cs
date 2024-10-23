using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities2Dto;

public static class Mappers
{
    internal static Mapper<FullUtilisateurDto, UtilisateurEntities> UtilisateurMapper { get; } = new Mapper<FullUtilisateurDto, UtilisateurEntities>();
    internal static Mapper<FullCaptureDetailDto, CaptureDetailsEntities> CaptureDetailMapper { get; } = new Mapper<FullCaptureDetailDto, CaptureDetailsEntities>();
    internal static Mapper<FullCaptureDto, CaptureEntities> CaptureMapper { get; } = new Mapper<FullCaptureDto, CaptureEntities>();
    internal static Mapper<FullEspeceDto, EspeceEntities> EspeceMapper { get; } = new Mapper<FullEspeceDto, EspeceEntities>();
    internal static Mapper<SuccessNormalDto, SuccessEntities> SuccessMapper { get; } = new Mapper<SuccessNormalDto, SuccessEntities>();
    internal static Mapper<LocalisationNormalDto, LocalisationEntities> LocalisationMapper { get; } = new Mapper<LocalisationNormalDto, LocalisationEntities>();
    internal static Mapper<HabitatNormalDto, HabitatEntities> HabitatMapper { get; } = new Mapper<HabitatNormalDto, HabitatEntities>();

    internal static void Reset()
    {
        UtilisateurMapper.Reset();
        CaptureDetailMapper.Reset();
        CaptureMapper.Reset();
        EspeceMapper.Reset();
        SuccessMapper.Reset();
        LocalisationMapper.Reset();
        HabitatMapper.Reset();
    }
}
