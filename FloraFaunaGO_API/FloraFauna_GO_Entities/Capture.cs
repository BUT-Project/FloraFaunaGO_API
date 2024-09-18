using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FloraFaunaGO_Modele;

public class Capture : IEquatable<Capture>
{
    private uint id;

    public uint Id
    {
        get { return id; }
        set
        {
            if (value < 0) id = 0;
            else id = value;
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

    private DateTime date_capture;
    public DateTime DateCapture { get; set; }

    private List<uint> LidCaptureDetail;

    public List<uint> LidCaptureDetails
    {
        get => LidCaptureDetails;
        internal set
        {
            if (value != null)
                LidCaptureDetail = value;
        }
    }

    private uint idEspece;
    public uint IdEspece
    {
        get => idEspece;
        set
        {
            if (value < 0) value = 0;
            idEspece = value;
        }
    }

    public Capture(uint id, Blob photo, DateTime dateCapture, List<uint> lidCaptureDetail, uint idEspece)
    {
        Id = id;
        Photo = photo;
        DateCapture = dateCapture;
        LidCaptureDetail = lidCaptureDetail;
        IdEspece = idEspece;
    }

    public bool Equals(Capture? other)
    {
        if (idEspece == other.idEspece) return true;
        return false;
    }
}
