namespace IshTap.Business.DTOs.Auth;

public class TokenResponseDto
{
    public string? Token { get; set; }
    public DateTime Expires { get; set; }
    public string? Username { get; set; }
}
