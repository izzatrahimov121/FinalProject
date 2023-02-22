﻿using Microsoft.AspNetCore.Http;

namespace IshTap.Business.DTOs.Auth;

public class AppUserDto
{
    public string Id { get; set; }
    public string Image { get; set; }
    public string Fullname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}
