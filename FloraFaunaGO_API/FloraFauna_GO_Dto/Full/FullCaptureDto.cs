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
        public FullCaptureDetailDto[]? CaptureDetails { get; set; }

        public FullEspeceDto Espece { get; set; }
    }
}
