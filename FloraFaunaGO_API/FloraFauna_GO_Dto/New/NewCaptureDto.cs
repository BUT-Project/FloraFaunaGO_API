using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.New;

public class NewCaptureDto
{
    public CaptureNormalDto Capture { get; set; }
    public UtilisateurNormalDto User { get; set; }
}
