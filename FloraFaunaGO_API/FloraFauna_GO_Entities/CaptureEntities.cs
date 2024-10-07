using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FloraFauna_GO_Entities;

public class CaptureEntities : BaseEntity
{
    public Blob Photo { get; set; }

    public uint Numero { get; set; }

    public ICollection<CaptureDetailsEntities> CaptureDetails = new Collection<CaptureDetailsEntities>();

    public EspeceEntities Espece = new EspeceEntities();
}
