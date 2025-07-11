using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonaKey.CoreLayer.Configuration;
using PersonaKey.EntityLayer.Concrete;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonaKey.CoreLayer.Services
{
    public class TokenService
    {
        private readonly JwtOptions _jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"),

                // Custom claims for policy control (as string: "True"/"False")
                new Claim("CanLogin", user.Role?.RoleAccess?.CanLogin == true ? "True" : "False"),
                new Claim("CanEditSite", user.Role?.RoleAccess?.CanEditSite == true ? "True" : "False")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtOptions.ExpireDays),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
