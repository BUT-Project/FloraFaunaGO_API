using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities
{
    public class SuccesStateEntities : BaseEntity
    {
        public double PercentSucces { get; set; }

        public bool IsSucces { get; set; } = false;

        [Required]
        public string SuccesEntitiesId { get; set; }

        public SuccesEntities SuccesEntities { get; set; }

        [Required]
        public string UtilisateurId { get; set; }

        public UtilisateurEntities User { get; set; }
    }
}
