using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Edit
{
    public class EditUserDto
    {
        public string? Pseudo { get; set; }

        public byte[]? Image { get; set; }

        public string? Mail { get; set; }
    }
}
