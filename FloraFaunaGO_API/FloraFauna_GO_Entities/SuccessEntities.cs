using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities
{
    public class SuccessEntities
    {
        [Required]
        public string Nom { get; set; }

        public int Avancement { get; set; } = 0;

        public string? Description { get; set; }
    }
}
