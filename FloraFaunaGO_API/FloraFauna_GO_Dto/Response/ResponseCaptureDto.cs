using System;
using FloraFauna_GO_Dto.Normal;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class ResponseCaptureDto
{
    public string? Id { get; set; }

    public FullCaptureDetailDto[]? CaptureDetails { get; set; }

    public FullEspeceDto Espece { get; set; }
}