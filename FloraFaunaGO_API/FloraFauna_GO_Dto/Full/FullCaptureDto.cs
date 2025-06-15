using FloraFauna_GO_Dto.Edit;
using FloraFauna_GO_Dto.Normal;

namespace FloraFauna_GO_Dto.Full
{
    public class FullCaptureDto
    {
        public ResponseCaptureDto Capture { get; set; }

        public List<FullCaptureDetailDto> CaptureDetails { get; set; } = new List<FullCaptureDetailDto>();

        public string idUtilisateur { get; set; }
    }
}
