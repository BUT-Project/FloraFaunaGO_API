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

        public string Type { get; set; }

        public string Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Objectif { get; set; }

        public string Evenenement { get; set; }

        public ICollection<SuccesStateEntities> SuccesStates { get; set; } = new List<SuccesStateEntities>();
    }
}
