using System.Reflection.Metadata;

namespace FloraFauna_GO_Dto.Normal;

public class CaptureNormalDto
{
    public Guid Id { get; set; }
    public Blob photo { get; set; }
    public DateTime DateCapture { get; set; }
}