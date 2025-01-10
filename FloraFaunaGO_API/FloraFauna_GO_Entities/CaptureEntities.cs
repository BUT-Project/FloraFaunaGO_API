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
    public byte[] Photo { get; set; }

    public uint Numero { get; set; }



    public CaptureEntities() { }
    public ICollection<CaptureDetailsEntities> CaptureDetails = new Collection<CaptureDetailsEntities>();

    public EspeceEntities Espece = new EspeceEntities();

    public bool AddCaptureDetail(CaptureDetailsEntities item)
    {
        bool found = false;
        CaptureDetails.ToList().ForEach(cde => {
            if (cde.Id == item.Id)
                found = true;
        });
        if(!found) CaptureDetails.Add(item);
        return found;
    }

    public bool RemoveCaptureDetails(CaptureDetailsEntities item)
    {
        bool found = false;
        CaptureDetails.ToList().ForEach(cde => {
            if (cde.Id == item.Id)
                found = true;
        });
        if (found) CaptureDetails.Remove(item);
        return found;
    }

    public CaptureDetailsEntities? RechCaptureDetails(string id)
    {
        CaptureDetailsEntities? captureDetails = null;
        bool found = false;
        CaptureDetails.ToList().ForEach(cde => {
            if (cde.Id == id)
                captureDetails = cde;
        });
        return captureDetails;
    }
}
