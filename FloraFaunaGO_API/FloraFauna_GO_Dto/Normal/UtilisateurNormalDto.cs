using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class UtilisateurNormalDto
{
    public string? Id { get; set; }
    public string? Pseudo { get; set; }

    public string? Mail { get; set; }

    public string Hash_mdp { get; set; }

    public DateTime DateInscription { get; set; }
}
