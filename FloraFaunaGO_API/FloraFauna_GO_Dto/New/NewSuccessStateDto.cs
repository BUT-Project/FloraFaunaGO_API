using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.New;

public class NewSuccessStateDto
{
    public SuccessStateNormalDto State { get; set; }
    public UtilisateurNormalDto Utilisateur { get; set; }
    public SuccessNormalDto Success { get; set; }
}
