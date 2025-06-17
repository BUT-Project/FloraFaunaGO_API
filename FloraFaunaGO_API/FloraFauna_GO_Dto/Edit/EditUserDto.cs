using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FloraFauna_GO_Dto.Edit
{
    public class EditUserDto
    {
        public string? Pseudo { get; set; }

        public IFormFile Image { get; set; }

        public string? Mail { get; set; }
    }
}
