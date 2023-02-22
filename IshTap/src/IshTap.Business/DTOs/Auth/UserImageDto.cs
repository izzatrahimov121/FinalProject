using Microsoft.AspNetCore.Http;

namespace IshTap.Business.DTOs.Auth;

public class UserImageDto
{
    public IFormFile Image { get; set; }
}
