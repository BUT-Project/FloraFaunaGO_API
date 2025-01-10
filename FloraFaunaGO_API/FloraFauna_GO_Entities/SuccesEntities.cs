using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities
{
    public class SuccesEntities : BaseEntity
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Description { get; set; }

        public double Objectif { get; set; }

        public ICollection<SuccesStateEntities> State { get; set; }
    }
}
