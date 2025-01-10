using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class SuccessNormalDto
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public double Objectif { get; set; } = 0;
}
