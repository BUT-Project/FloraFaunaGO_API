using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto;

public class UtilisateurDto
{
    public Guid Id { get; set; }
    public string? Pseudo { get; set; }

    public string? Mail { get; set; }

    public string Hash_mdp { get; set; }

    public DateTime DateInscription { get; set; }

    //public IEnumerable<CaptureDto> Captures { get; set; }
}
