using FloraFauna_GO_Dto.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Full;

public class FullEspeceDto
{
    public EspeceNormalDto Espece { get; set; }
    public LocalisationNormalDto[]? localisationNormalDtos { get; set; }


}
