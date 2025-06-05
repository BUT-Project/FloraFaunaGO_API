using FloraFauna_GO_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class SuccessNormalDto
{
    public string? Id { get; set; }
    public string Nom { get; set; }
    public string Type { get; set; }
    public string? Image { get; set; }
    public string Description { get; set; }
    public double Objectif { get; set; }
    public string Evenement { get; set; }
}
