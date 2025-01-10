using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FloraFauna_GO_Entities;

public class CaptureEntities : BaseEntity
{
    public byte[] Photo { get; set; }

    public uint Numero { get; set; }

    public ICollection<CaptureDetailsEntities> CaptureDetails = new Collection<CaptureDetailsEntities>();

    [Required]
    public string EspeceId { get; set;  }

    public EspeceEntities Espece { get; set; }

    [Required]
    public string UtilisateurId { get; set; }

    public UtilisateurEntities Utilisateur { get; set; }
}
