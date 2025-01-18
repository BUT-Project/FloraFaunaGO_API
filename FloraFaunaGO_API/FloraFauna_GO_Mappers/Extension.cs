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

public static class Extension
{
    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            set.Add(item);
        }
    }

    public static LocalisationEntities ToEntities(this LocalisationNormalDto dto)
    {
        Func<LocalisationNormalDto, LocalisationEntities> creator = (dto) => new LocalisationEntities()
        {
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Rayon = dto.Rayon
        };

        return dto.ToU(Mappers.LocalisationMapper, creator);
    }

    public static LocalisationNormalDto ToDto(this LocalisationEntities entities)
    {
        Func<LocalisationEntities, LocalisationNormalDto> creator = (entities) => new LocalisationNormalDto()
        {
            Latitude = entities.Latitude,
            Longitude = entities.Longitude,
            Rayon = entities.Rayon
        };

        return entities.ToT(null, creator, null);
    }

    public static CaptureEntities ToEntities(this FullCaptureDto dto)
    {
        Func<FullCaptureDto, CaptureEntities> creator = (dto) => new CaptureEntities()
        {
            Id = dto.Capture.Id,
            Photo = dto.Capture.photo,
            Espece = ToEntities(dto.Espece),
            CaptureDetails = dto.CaptureDetails.Select(cd => ToEntities(cd)).ToList(),
        };
        return dto.ToU(Mappers.CaptureMapper, creator);
    }

    public static FullCaptureDto ToDto(this CaptureEntities entities)
    {
        Func<CaptureEntities, FullCaptureDto> creator = (entities) => new FullCaptureDto()
        {
            Capture = new CaptureNormalDto() { Id = entities.Id, photo = entities.Photo },
            Espece = ToDto(entities.Espece),
            CaptureDetails = entities.CaptureDetails.Select(cd => ToDto(cd)).ToArray(),
        };
        return entities.ToT(null,creator, null);
    }

    public static CaptureDetailsEntities ToEntities(this FullCaptureDetailDto dto) 
    {
        Func<FullCaptureDetailDto, CaptureDetailsEntities> creator = (dto) => new CaptureDetailsEntities()
        {
            Id = dto.CaptureDetail.Id,
            Shiny = dto.CaptureDetail.Shiny,
            Localisation = ToEntities(dto.localisationNormalDtos)
        };
        return dto.ToU(Mappers.CaptureDetailMapper, creator);
    }

    public static FullCaptureDetailDto ToDto(this CaptureDetailsEntities entities) 
    {
        Func<CaptureDetailsEntities, FullCaptureDetailDto> creator = (entities) => new FullCaptureDetailDto()
        {
            CaptureDetail = new CaptureDetailNormalDto() { Id = entities.Id, Shiny = entities.Shiny },
            localisationNormalDtos = ToDto(entities.Localisation),
        };
        return entities.ToT(null, creator, null);
    }

    public static EspeceEntities ToEntities(this FullEspeceDto dto)
    {
        Func<FullEspeceDto, EspeceEntities> creator = (dto) => new EspeceEntities()
        {
            Nom = dto.Espece.Nom,
            Nom_scientifique = dto.Espece.Nom_Scientifique,
            Description = dto.Espece.Description,
            Image = dto.Espece.Image,
            Image3D = dto.Espece.Image3D,
            Localisations = dto.localisationNormalDtos.Select(loc => ToEntities(loc)).ToList(),
            Climat = dto.Espece.Climat,
            Zone = dto.Espece.Zone,
            Famille = dto.Espece.Famille,
            Regime = dto.Espece.Regime,
        };
        return dto.ToU(Mappers.EspeceMapper, creator);
    }
    public static FullEspeceDto ToDto(this EspeceEntities entities)
    {
        Func<EspeceEntities, FullEspeceDto> creator = (entities) => new FullEspeceDto()
        {
            Espece = new EspeceNormalDto()
            {
                Id = entities.Id,
                Description = entities.Description ?? "",
                Image = entities.Image,
                Image3D = entities.Image3D,
                Nom = entities.Nom,
                Nom_Scientifique = entities.Nom_scientifique,
                Climat = entities.Climat,
                Zone = entities.Zone,
                Regime = entities.Regime,
            },
            localisationNormalDtos = entities.Localisations.Select(loc => ToDto(loc)).ToArray(),
        };
        return entities.ToT(null, creator);
    }

    public static UtilisateurEntities ToEntities(this FullUtilisateurDto dto)
    {
        Func<FullUtilisateurDto, UtilisateurEntities> creator = (dto) => new UtilisateurEntities()
        {
            Id = dto.Utilisateur.Id,
            Pseudo = dto.Utilisateur.Pseudo,
            Mail = dto.Utilisateur.Mail,
            Hash_mdp = dto.Utilisateur.Hash_mdp,
            DateInscription = dto.Utilisateur.DateInscription,
            Captures = dto.Capture?.Select(c => ToEntities(c)).ToList(),
            SuccesState = dto.SuccessState?.Select(s => ToEntities(s)).ToList(),
        };
        return dto.ToU(Mappers.UtilisateurMapper, creator);
    }

    public static FullUtilisateurDto ToDto(this UtilisateurEntities entities)
    {
        Func<UtilisateurEntities, FullUtilisateurDto> creator = (entities) => new FullUtilisateurDto()
        {
            Utilisateur = new UtilisateurNormalDto()
            {
                Pseudo = entities.Pseudo,
                Mail = entities.Mail,
                Hash_mdp = entities.Hash_mdp,
                DateInscription = entities.DateInscription,
                Id = entities.Id,
            },
            Capture = entities.Captures.Select(cpt => ToDto(cpt)).ToArray(),
            SuccessState = entities.SuccesState.Select(s => ToDto(s)).ToArray(),
        };
        return entities.ToT(null, creator);
    }

    public static SuccesEntities ToEntities(this SuccessNormalDto dto)
    {
        Func<SuccessNormalDto, SuccesEntities> creator = (dto) => new SuccesEntities()
        {
            Nom = dto.Nom,
            Objectif = dto.Objectif,
            Description = dto.Description,
        };
        return dto.ToU(Mappers.SuccessMapper, creator);
    }

    public static SuccessNormalDto ToDto(this SuccesEntities entities)
    {
        Func<SuccesEntities, SuccessNormalDto> creator = (entities) => new SuccessNormalDto()
        {
            Objectif = entities.Objectif,
            Description = entities.Description,
            Nom = entities.Nom,
        };
        return entities.ToT(null, creator);
    }

    public static SuccesStateEntities ToEntities(this FullSuccessStateDto dto)
    {
        Func<FullSuccessStateDto, SuccesStateEntities> creator = (dto) => new SuccesStateEntities()
        {
            PercentSucces = dto.State.PercentSucces,
            IsSucces = dto.State.IsSucces,
            SuccesEntities = dto.Success.ToEntities(),
        };
        return dto.ToU(Mappers.SuccessStateMapper, creator);
    }

    public static FullSuccessStateDto ToDto(this SuccesStateEntities entities)
    {
        Func<SuccesStateEntities, FullSuccessStateDto> creator = (entities) => new FullSuccessStateDto()
        {
            State = new SuccessStateNormalDto()
            {
                PercentSucces = entities.PercentSucces,
                IsSucces = entities.IsSucces,
            },
            Success = entities.SuccesEntities.ToDto(),
        };
        return entities.ToT(null, creator);
    }



    public static Pagination<FullEspeceDto> ToPagingResponseDtos(this Pagination<EspeceEntities> entities)
    {
        return new Pagination<FullEspeceDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToDto).ToArray()
        };
    }

    public static Pagination<FullCaptureDto> ToPagingResponseDtos(this Pagination<CaptureEntities> entities)
    {
        return new Pagination<FullCaptureDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToDto).ToArray()
        };
    }

    public static Pagination<FullCaptureDetailDto> ToPagingResponseDtos(this Pagination<CaptureDetailsEntities> entities)
    {
        return new Pagination<FullCaptureDetailDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToDto).ToArray()
        };
    }

    public static Pagination<FullUtilisateurDto> ToPagingResponseDtos(this Pagination<UtilisateurEntities> entities)
    {
        return new Pagination<FullUtilisateurDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToDto).ToArray()
        };
    }

    public static Pagination<FullSuccessStateDto> ToPagingResponseDtos(this Pagination<SuccesStateEntities> entities)
    {
        return new Pagination<FullSuccessStateDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToDto).ToArray()
        };
    }

    public static Pagination<SuccessNormalDto> ToPagingResponseDtos(this Pagination<SuccesEntities> entities)
    {
        return new Pagination<SuccessNormalDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToDto).ToArray()
        };
    }
}
