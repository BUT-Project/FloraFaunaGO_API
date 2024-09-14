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
            if (Blob.Equals(value, null)) photo = default;
            else photo = value;
        }
    }

    private DateTime date_capture;
    public DateTime DateCapture { get; set; }
}
