namespace FloraFauna_GO_Dto;

public class ResetPasswordDto
{
    public string Mail { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}
