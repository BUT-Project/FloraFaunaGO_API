using FloraFauna_GO_Dto.Normal;

namespace FloraFauna_GO_Dto.Full;

public class FullEspeceDto
{
    public string? Id { get; set; }

    public string Nom { get; set; }

    public string Nom_Scientifique { get; set; }

    public string Description { get; set; }

    public byte[]? Image { get; set; }

    public byte[]? Image3D { get; set; }

    public string Famille { get; set; }

    public string Zone { get; set; }

    public string Climat { get; set; }

    public string Class { get; set; }

    public string Kingdom { get; set; }

    public string Regime { get; set; }
    public LocalisationNormalDto[]? localisations { get; set; }


}
