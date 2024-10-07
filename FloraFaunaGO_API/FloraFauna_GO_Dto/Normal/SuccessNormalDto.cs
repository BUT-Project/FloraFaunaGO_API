using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class SuccessNormalDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Avancement { get; set; } = 0;
}
