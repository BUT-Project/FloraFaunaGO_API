using FloraFauna_GO_Dto.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Full
{
    public class FullCaptureDto
    {
        public CaptureNormalDto Capture { get; set; }

        public List<FullCaptureDetailDto> CaptureDetails { get; set; } = new List<FullCaptureDetailDto>();

        public string idUtilisateur { get; set; }
    }
}
