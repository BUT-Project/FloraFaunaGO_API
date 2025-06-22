using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraFauna_GO_Entities
{
    public class CustomRole : IdentityRole<string>
    {
        [Column(TypeName = "varchar(255)")]
        public override string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
