using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FloraFaunaGO_Modele;

public class Capture
{
    private Guid id;

    public Guid Id
    {
        get;
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

    private DateTime date_capture;
    public DateTime DateCapture { get; set; }

    private List<CaptureDetails> captureDetails;

    public List<CaptureDetails> CaptureDetails => captureDetails;

    private Espece espece;
    public Espece Espece
    {
        get; set;
    }

    public Capture(Blob photo, DateTime dateCapture, Espece espece)
    {
        Photo = photo;
        DateCapture = dateCapture;
        captureDetails = new List<CaptureDetails>();
        Espece = espece;
    }
}
