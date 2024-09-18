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

    public CaptureEntities(Blob photo, DateTime dateCapture, EspeceEntities espece)
    {
        Photo = photo;
        DateCapture = dateCapture;
        captureDetails = new List<CaptureDetailsEntities>();
        Espece = espece;
    }
}
