using System.ComponentModel.DataAnnotations;

namespace FloraFauna_GO_Entities;

public class CaptureEntities : BaseEntity
{
    public byte[] Photo { get; set; }

    public uint Numero { get; set; }

    public ICollection<CaptureDetailsEntities> CaptureDetails = new List<CaptureDetailsEntities>();

    [Required]
    public string EspeceId { get; set; }

    public EspeceEntities Espece { get; set; }

    [Required]
    public string UtilisateurId { get; set; }

    public UtilisateurEntities Utilisateur { get; set; }
}
