using IshTap.Business.DTOs.Auth;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IshTap.Business.HelperServices.Implementations;

public class TokenHandler : ITokenHandler
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public TokenHandler(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<TokenResponseDto> GenerateTokenAsync(AppUser user, int minute)
    {
        List<Claim> claims = new()
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
            };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));

        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(minute),
            signingCredentials: signingCredentials
        );

        //JwtSecurityTokenHandler tokenHandler = new();
        //var token = tokenHandler.WriteToken(jwtSecurityToken);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        TokenResponseDto tokenResponse = new()
        {
            Token = token,
            Expires = jwtSecurityToken.ValidTo,
            Username = user.UserName
        };
        return tokenResponse;
    }
}
