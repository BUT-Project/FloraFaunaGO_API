using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto;

public class ResetPasswordDto
{
    public string Mail { get; set; }
    public string NewPassword { get; set; }
}
