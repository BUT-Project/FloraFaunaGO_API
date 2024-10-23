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

    public static HabitatEntities ToEntities(this HabitatNormalDto dto)
    {
        Func<HabitatNormalDto, HabitatEntities> creator = (dto) => new HabitatEntities()
            {
                Id = dto.Id,
                Zone = dto.Zone,
                Climat = dto.Climat
            };

        return dto.ToU(Mappers.HabitatMapper, creator);
    }

    public static HabitatNormalDto ToDto(this HabitatEntities entities)
    {
        Func<HabitatEntities, HabitatNormalDto> creator = (entities) => new HabitatNormalDto()
        {
            Id = entities.Id,
            Zone = entities.Zone,
            Climat = entities.Climat
        };

        return entities.ToT(null, creator, null);
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
            Espece = ToEntites(dto.Espece),
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

    public static EspeceEntities ToEntites(this FullEspeceDto dto)
    {
        Func<FullEspeceDto, EspeceEntities> creator = (dto) => new EspeceEntities()
        {
            Nom = dto.Espece.Nom,
            Nom_scientifique = dto.Espece.Nom_Scientifique,
            Description = dto.Espece.Description,
            Image = dto.Espece.Image,
            Image3D = dto.Espece.Image3D,
            Famille = dto.Famille,
            Habitats = dto.Habitats.Select(hab => ToEntities(hab)).ToList(),
            Localisations = dto.localisationNormalDtos.Select(loc => ToEntities(loc)).ToList(),
            Regime = dto.Regime_Alimentaire,
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
                Description = entities.Description,
                Image = entities.Image,
                Image3D = entities.Image3D,
                Nom = entities.Nom,
                Nom_Scientifique = entities.Nom_scientifique
            },
            Habitats = entities.Habitats.Select(hab => ToDto(hab)).ToArray(),
            Regime_Alimentaire = entities.Regime,
            Famille = entities.Famille,
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
        };
        return entities.ToT(null, creator);
    }

    public static SuccessEntities ToEntities(this SuccessNormalDto dto)
    {
        Func<SuccessNormalDto, SuccessEntities> creator = (dto) => new SuccessEntities()
        {
            Nom = dto.Name,
            Avancement = dto.Avancement,
            Description = dto.Description,
        };
        return dto.ToU(Mappers.SuccessMapper, creator);
    }

    public static SuccessNormalDto ToDto(this SuccessEntities entities)
    {
        Func<SuccessEntities, SuccessNormalDto> creator = (entities) => new SuccessNormalDto()
        {
            Avancement = entities.Avancement,
            Description = entities.Description,
            Name = entities.Nom,
        };
        return entities.ToT(null, creator);
    }
}
