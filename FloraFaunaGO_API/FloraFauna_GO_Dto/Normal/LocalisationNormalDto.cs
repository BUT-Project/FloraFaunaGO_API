using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

﻿namespace FloraFauna_GO_Dto.Normal;

public class LocalisationNormalDto
{
    public string? Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double Exactitude { get; set; }

    public double Rayon { get; set; }
}
