using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Edit
{
    public class ResponseCaptureDto
    {
        public string? Id { get; set; }

        public string IdEspece { get; set; }
        public byte[]? photo { get; set; }
    }
}
