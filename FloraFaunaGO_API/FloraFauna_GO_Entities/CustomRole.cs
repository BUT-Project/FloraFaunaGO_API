using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities
{
    public class CustomRole : IdentityRole<string>
    {
        [Column(TypeName = "varchar(255)")]
        public override string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
