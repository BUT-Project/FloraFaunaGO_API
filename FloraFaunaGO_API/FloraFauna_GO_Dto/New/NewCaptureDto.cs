using FloraFauna_GO_Dto.Full;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.New;

public class NewCaptureDto
{
    public FullCaptureDto Capture { get; set; }
    public FullUtilisateurDto User { get; set; }
}
