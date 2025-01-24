using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities
{
    public class ZoneEntities
    {
        public string zoneIndication { get; set; }

        public ICollection<EspeceEntities> entities { get; set; }
    }
}
