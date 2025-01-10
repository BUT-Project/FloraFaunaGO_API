using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto;

public class CaptureDto
{
    public Guid Id { get; set; }
    public byte[] photo { get; set; }
    public DateTime DateCapture { get; set; }
    public IEnumerable<CaptureDetailDto> CaptureDetailDtos { get; set; } = Enumerable.Empty<CaptureDetailDto>();

    //public EspeceDto Espece { get; set; }
}
