using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class SuccessStateNormalDto
{
    public string? Id { get; set; }
    public double PercentSucces { get; set; }
    public bool IsSucces { get; set; } = false;
}
