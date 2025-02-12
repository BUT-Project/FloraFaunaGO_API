using FloraFauna_GO_Dto.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.New;

public class NewEspeceDto
{
    public EspeceNormalDto espece { get; set; }
    public LocalisationNormalDto[] localisation { get; set; }
}
