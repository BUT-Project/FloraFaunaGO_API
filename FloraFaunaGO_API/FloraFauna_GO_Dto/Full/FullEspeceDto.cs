using FloraFauna_GO_Dto.Normal;
using FloraFaunaGO_Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Full;

public class FullEspeceDto
{
    public EspeceNormalDto Espece { get; set; }
    public HabitatNormalDto[]? Habitats { get; set; }
    public Regime_Alimentaire Regime_Alimentaire { get; set; }
    public Famille Famille { get; set; }
    public LocalisationNormalDto[]? localisationNormalDtos { get; set; }


}
