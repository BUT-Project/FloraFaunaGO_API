namespace FloraFauna_GO_Entities;

public class LocalisationEntities : BaseEntity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Rayon { get; set; }
    public double Altitude { get; set; }
    public double Exactitude { get; set; }

    public ICollection<EspeceLocalisationEntities>? EspeceLocalisation { get; set; } = new List<EspeceLocalisationEntities>();
    public CaptureDetailsEntities? CapturesDetail { get; set; }
    public string? CaptureDetailsId { get; set; }
}
