using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Normal;

public class CaptureNormalDto
{
    public Guid Id { get; set; }
    public Blob photo { get; set; }
    public DateTime DateCapture { get; set; }
}
