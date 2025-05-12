using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ICollection<SuccesStateEntities> SuccesStates { get; set; } = new List<SuccesStateEntities>();
    }
}
