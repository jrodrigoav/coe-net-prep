using Backend.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.API.Services
{
    public class AuthTokenService
    {
        private readonly JwtSettings _settings;

        public AuthTokenService(IOptionsMonitor<JwtSettings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;
        }
         public JwtSecurityToken GenerateToken(string username, Claim[]? additionalClaims = null)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub,username),
            // this guarantees the token is unique
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            if (additionalClaims is object)
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                expires: DateTime.UtcNow.Add(_settings.Expire),
                claims: claims,
                signingCredentials: creds
            );
        }


    }
}
