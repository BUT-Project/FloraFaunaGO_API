using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities
{
    public class ClimatEntitites : BaseEntity
    {
        public string climatName { get; set; }

        public ICollection<EspeceEntities> entities { get; set; }
    }
}
