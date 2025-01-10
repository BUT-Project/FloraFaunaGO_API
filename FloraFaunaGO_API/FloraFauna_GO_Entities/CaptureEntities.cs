using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace FloraFauna_GO_Entities;

public class CaptureEntities : BaseEntity
{
    public ICollection<CaptureDetailsEntities> CaptureDetails = new Collection<CaptureDetailsEntities>();

    public EspeceEntities Espece = new();

    public Blob Photo { get; set; }

    public uint Numero { get; set; }

    public bool AddCaptureDetail(CaptureDetailsEntities item)
    {
        var found = false;
        CaptureDetails.ToList().ForEach(cde =>
        {
            if (cde.Id == item.Id)
                found = true;
        });
        if (!found) CaptureDetails.Add(item);
        return found;
    }

    public bool RemoveCaptureDetails(CaptureDetailsEntities item)
    {
        var found = false;
        CaptureDetails.ToList().ForEach(cde =>
        {
            if (cde.Id == item.Id)
                found = true;
        });
        if (found) CaptureDetails.Remove(item);
        return found;
    }

    public CaptureDetailsEntities? RechCaptureDetails(string id)
    {
        CaptureDetailsEntities? captureDetails = null;
        var found = false;
        CaptureDetails.ToList().ForEach(cde =>
        {
            if (cde.Id == id)
                captureDetails = cde;
        });
        return captureDetails;
    }
}