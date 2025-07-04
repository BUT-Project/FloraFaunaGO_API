﻿namespace FloraFauna_GO_Dto.Normal;

public class CaptureNormalDto
{
    public string? Id { get; set; }

    public string IdEspece { get; set; }
    public byte[]? photo { get; set; }

    public LocalisationNormalDto? LocalisationNormalDto { get; set; }

    public bool? Shiny { get; set; }
}
