using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto;

public class EspeceDto
{
    public Guid Id { get; set; }

    public string? Nom { get; set; }

    public string? Nom_Scientifique { get; set; }

    public string Description { get; set; }

    public int Numero { get; set; }

    public Blob Image { get; set; }

    public IEnumerable<HabitatDto> Habitats { get; set; } = Enumerable.Empty<HabitatDto>();
    public IEnumerable<LocalisationDto> Localisations { get; set; } = Enumerable.Empty<LocalisationDto>();
    
    // enum à rajouter
}
