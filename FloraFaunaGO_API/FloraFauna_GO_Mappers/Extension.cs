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

    public static LocalisationEntities ToEntities(this LocalisationNormalDto dto, string id)
    {
        Func<LocalisationNormalDto, LocalisationEntities> creator = (dto) => new LocalisationEntities()
        {
            Id = id,
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
            Rayon = entities.Rayon,
            Id = entities.Id,
        };

        return entities.ToT(null, creator, null);
    }

    public static CaptureEntities ToEntities(this CaptureNormalDto dto)
    {
        Func<CaptureNormalDto, CaptureEntities> creator = (dto) => new CaptureEntities()
        {
            Photo = dto.photo,
            EspeceId = dto.IdEspece,
            CaptureDetails = new List<CaptureDetailsEntities>()
            {
                new CaptureDetailsEntities()
                {
                    Localisation = dto.LocalisationNormalDto != null ? dto.LocalisationNormalDto.ToEntities() : null,
                    Shiny = dto.Shiny ?? false
                }
            },
        };
        return dto.ToU(Mappers.CaptureMapper, creator);
    }

    public static CaptureEntities ToEntities(this CaptureNormalDto dto, string id)
    {
        Func<CaptureNormalDto, CaptureEntities> creator = (dto) => new CaptureEntities()
        {
            Id = id,
            Photo = dto.photo,
            EspeceId = dto.IdEspece
        };
        return dto.ToU(Mappers.CaptureMapper, creator);
    }

    public static CaptureNormalDto ToDto(this CaptureEntities entities)
    {
        Func<CaptureEntities, CaptureNormalDto> creator = (entities) => new CaptureNormalDto()
        {
            photo = entities.Photo,
            Id = entities.Id,
        };
        return entities.ToT(null, creator, null);
    }

    public static FullCaptureDto ToResponseDto(this CaptureEntities entities)
    {
        Func<CaptureEntities, FullCaptureDto> creator = (entities) => new FullCaptureDto()
        {
            Capture = new CaptureNormalDto()
            {
                Id = entities.Id,
                photo = entities.Photo,
            },
        };
        Action<CaptureEntities, FullCaptureDto> linker = (entities, dto) =>
        {
            dto.Capture.IdEspece = entities.EspeceId;
            dto.idUtilisateur = entities.UtilisateurId;
        };
        return entities.ToT(null,creator, linker);
    }

    public static CaptureDetailsEntities ToEntities(this CaptureDetailNormalDto dto) 
    {
        Func<CaptureDetailNormalDto, CaptureDetailsEntities> creator = (dto) => new CaptureDetailsEntities()
        {
            Shiny = dto.Shiny,
        };
        return dto.ToU(Mappers.CaptureDetailMapper, creator);
    }

    public static CaptureDetailsEntities ToEntities(this CaptureDetailNormalDto dto, string id)
    {
        Func<CaptureDetailNormalDto, CaptureDetailsEntities> creator = (dto) => new CaptureDetailsEntities()
        {
            Id = id,
            Shiny = dto.Shiny,
        };
        return dto.ToU(Mappers.CaptureDetailMapper, creator);
    }

    public static CaptureDetailNormalDto ToDto(this CaptureDetailsEntities entities) 
    {
        Func<CaptureDetailsEntities, CaptureDetailNormalDto> creator = (entities) => new CaptureDetailNormalDto()
        {
            Shiny = entities.Shiny,
            Id = entities.Id,
        };
        return entities.ToT(null, creator, null);
    }

    public static FullCaptureDetailDto ToResponseDto(this CaptureDetailsEntities entities)
    {
        Func<CaptureDetailsEntities, FullCaptureDetailDto> creator = (entities) => new FullCaptureDetailDto()
        {
            CaptureDetail = new CaptureDetailNormalDto()
            {
                Id = entities.Id,
                Shiny = entities.Shiny,
            },
        };
        Action<CaptureDetailsEntities, FullCaptureDetailDto> linker = (entities, dto) =>
        {
            dto.localisationNormalDtos = new LocalisationNormalDto() { Id = entities.LocalisationId };
        };
        return entities.ToT(null, creator, linker);
    }

    public static EspeceEntities ToEntities(this EspeceNormalDto dto)
    {
        Func<EspeceNormalDto, EspeceEntities> creator = (dto) => new EspeceEntities()
        {
            Nom = dto.Nom,
            Nom_scientifique = dto.Nom_Scientifique,
            Description = dto.Description,
            Image = dto.Image,
            Image3D = dto.Image3D,
            Climat = dto.Climat,
            Zone = dto.Zone,
            Famille = dto.Famille,
            Regime = dto.Regime,
        };
        return dto.ToU(Mappers.EspeceMapper, creator);
    }

    public static EspeceEntities ToEntities(this EspeceNormalDto dto, string id)
    {
        Func<EspeceNormalDto, EspeceEntities> creator = (dto) => new EspeceEntities()
        {
            Id = id,
            Nom = dto.Nom,
            Nom_scientifique = dto.Nom_Scientifique,
            Description = dto.Description,
            Image = dto.Image,
            Image3D = dto.Image3D,
            Climat = dto.Climat,
            Zone = dto.Zone,
            Famille = dto.Famille,
            Regime = dto.Regime,
        };
        return dto.ToU(Mappers.EspeceMapper, creator);
    }
    public static EspeceNormalDto ToDto(this EspeceEntities entities)
    {
        Func<EspeceEntities, EspeceNormalDto> creator = (entities) => new EspeceNormalDto()
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
        };
        return entities.ToT(null, creator);
    }

    public static FullEspeceDto ToResponseDto(this EspeceEntities entities)
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
        };
        Action<EspeceEntities, FullEspeceDto> linker = (entities, dto) =>
        {
            dto.localisationNormalDtos = entities.Localisations.Select(loc => new LocalisationNormalDto() { Id = loc.LocalisationId }).ToArray();
        };
        return entities.ToT(null, creator, linker);
    }

    public static UtilisateurEntities ToEntities(this UtilisateurNormalDto dto)
    {
        Func<UtilisateurNormalDto, UtilisateurEntities> creator = (dto) => new UtilisateurEntities()
        {
            Pseudo = dto.Pseudo,
            Mail = dto.Mail,
            Hash_mdp = dto.Hash_mdp,
            DateInscription = dto.DateInscription,
        };

        return dto.ToU(Mappers.UtilisateurMapper, creator);
    }

    public static UtilisateurEntities ToEntities(this UtilisateurNormalDto dto, string id)
    {
        Func<UtilisateurNormalDto, UtilisateurEntities> creator = (dto) => new UtilisateurEntities()
        {
            Id = id,
            Pseudo = dto.Pseudo,
            Mail = dto.Mail,
            Hash_mdp = dto.Hash_mdp,
            DateInscription = dto.DateInscription,
        };

        return dto.ToU(Mappers.UtilisateurMapper, creator);
    }

    public static UtilisateurNormalDto ToDto(this UtilisateurEntities entities)
    {
        Func<UtilisateurEntities, UtilisateurNormalDto> creator = (entities) => new UtilisateurNormalDto()
        {
            Pseudo = entities.Pseudo,
            Mail = entities.Mail,
            Hash_mdp = entities.Hash_mdp,
            DateInscription = entities.DateInscription,
            Id = entities.Id,
        };
        return entities.ToT(null, creator);
    }

    public static FullUtilisateurDto ToResponseDto(this UtilisateurEntities entities)
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
        };
        Action<UtilisateurEntities, FullUtilisateurDto> linker = (entities, dto) =>
        {
            dto.Capture = entities.Captures.Select(c => c.ToDto()).ToArray();
            dto.SuccessState = entities.SuccesState.Select(s => s.ToDto()).ToArray();
        };
        return entities.ToT(null, creator, linker);
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

    public static SuccesEntities ToEntities(this SuccessNormalDto dto, string id)
    {
        Func<SuccessNormalDto, SuccesEntities> creator = (dto) => new SuccesEntities()
        {
            Id = id,
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
            Id = entities.Id,
        };
        return entities.ToT(null, creator);
    }

    public static SuccesStateEntities ToEntities(this SuccessStateNormalDto dto)
    {
        Func<SuccessStateNormalDto, SuccesStateEntities> creator = (dto) => new SuccesStateEntities()
        {
            PercentSucces = dto.PercentSucces,
            IsSucces = dto.IsSucces,
        };
        return dto.ToU(Mappers.SuccessStateMapper, creator);
    }

    public static SuccesStateEntities ToEntities(this SuccessStateNormalDto dto, string id)
    {
        Func<SuccessStateNormalDto, SuccesStateEntities> creator = (dto) => new SuccesStateEntities()
        {
            Id = id,
            PercentSucces = dto.PercentSucces,
            IsSucces = dto.IsSucces,
        };
        return dto.ToU(Mappers.SuccessStateMapper, creator);
    }

    public static SuccessStateNormalDto ToDto(this SuccesStateEntities entities)
    {
        Func<SuccesStateEntities, SuccessStateNormalDto> creator = (entities) => new SuccessStateNormalDto()
        {
            PercentSucces = entities.PercentSucces,
            IsSucces = entities.IsSucces,
            Id = entities.Id,
        };
        return entities.ToT(null, creator);
    }

    public static FullSuccessStateDto ToResponseDto(this SuccesStateEntities entities)
    {
        Func<SuccesStateEntities, FullSuccessStateDto> creator = (entities) => new FullSuccessStateDto()
        {
            State = new SuccessStateNormalDto()
            {
                Id = entities.Id,
                PercentSucces = entities.PercentSucces,
                IsSucces = entities.IsSucces,
            },
        };
        return entities.ToT(null, creator, null);
    }

    public static Pagination<FullEspeceDto> ToPagingResponseDtos(this Pagination<EspeceEntities> entities)
    {
        return new Pagination<FullEspeceDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToResponseDto).ToArray()
        };
    }

    public static Pagination<FullCaptureDto> ToPagingResponseDtos(this Pagination<CaptureEntities> entities)
    {
        return new Pagination<FullCaptureDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToResponseDto).ToArray()
        };
    }

    public static Pagination<FullCaptureDetailDto> ToPagingResponseDtos(this Pagination<CaptureDetailsEntities> entities)
    {
        return new Pagination<FullCaptureDetailDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToResponseDto).ToArray()
        };
    }

    public static Pagination<FullUtilisateurDto> ToPagingResponseDtos(this Pagination<UtilisateurEntities> entities)
    {
        return new Pagination<FullUtilisateurDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToResponseDto).ToArray()
        };
    }

    public static Pagination<FullSuccessStateDto> ToPagingResponseDtos(this Pagination<SuccesStateEntities> entities)
    {
        return new Pagination<FullSuccessStateDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToResponseDto).ToArray()
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

    public static Pagination<LocalisationNormalDto> ToPagingResponseDtos(this Pagination<LocalisationEntities> entities)
    {
        return new Pagination<LocalisationNormalDto>
        {
            PageIndex = entities.PageIndex,
            CountPerPage = entities.CountPerPage,
            TotalCount = entities.TotalCount,
            Items = entities.Items.Select(ToDto).ToArray()
        };
    }
}
