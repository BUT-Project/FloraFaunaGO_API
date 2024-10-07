using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FloraFaunaGO_Modele;

public class CaptureEntities
{
    private Guid id;

    public Guid Id { get; }

    private uint numero;
    public uint Numero
    {
        get { return numero; }
        set
        {
            if (value < 0) numero = 0;
            else numero = value;
        }
    }

    private Blob photo;
    public Blob Photo
    {
        get { return photo; }
        set
        {
            if (Equals(value, null)) photo = default;
            else photo = value;
        }
    }

    private DateTime dateCapture;
    public DateTime DateCapture { get; set; }

    private List<CaptureDetailsEntities> captureDetails;

    public List<CaptureDetailsEntities> CaptureDetails => captureDetails;

    private EspeceEntities espece;
    public EspeceEntities Espece {
        get => espece;
        set
        {
            if (value == null || espece == value) return;
            espece = value;
        }
    }

    public bool AddCaptureDetail(CaptureDetailsEntities item)
    {
        bool found = false;
        CaptureDetails.ForEach(cde => {
            if (cde.Id == item.Id)
                found = true;
        });
        if(!found) CaptureDetails.Add(item);
        return found;
    }

    public bool RemoveCaptureDetails(CaptureDetailsEntities item)
    {
        bool found = false;
        CaptureDetails.ForEach(cde => {
            if (cde.Id == item.Id)
                found = true;
        });
        if (found) CaptureDetails.Remove(item);
        return found;
    }

    public CaptureDetailsEntities? RechCaptureDetails(Guid id)
    {
        CaptureDetailsEntities? captureDetails = null;
        bool found = false;
        CaptureDetails.ForEach(cde => {
            if (cde.Id == id)
                captureDetails = cde;
        });
        return captureDetails;
    }

    public CaptureEntities() { }
}
