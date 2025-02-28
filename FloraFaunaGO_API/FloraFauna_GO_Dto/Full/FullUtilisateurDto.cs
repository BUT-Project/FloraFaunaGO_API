using FloraFauna_GO_Dto.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Full;

public class FullUtilisateurDto
{
    public UtilisateurNormalDto Utilisateur { get; set; }
    public CaptureNormalDto[]? Capture { get; set; }

    public SuccessStateNormalDto[]? SuccessState { get; set; }
}
