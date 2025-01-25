using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities
{
    public class EspeceLocalisationEntities
    {
        public string EspeceId { get; set; }
        public EspeceEntities Espece { get; set; }

        public string LocalisationId { get; set; }
        public LocalisationEntities Localisation { get; set; }
    }
}
