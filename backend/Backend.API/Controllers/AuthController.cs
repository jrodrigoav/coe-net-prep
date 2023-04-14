using Backend.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.API.Controllers
{
    [ApiController, Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous, HttpPost("login/{username}")]
        public object Authenticate(string username, [FromServices]AuthTokenService service)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("username", username));
            claims.Add(new Claim("displayname", username));

            // Add roles as multiple claims
            //foreach(var role in user.Roles) 
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role.Name));
            //}
            // Optionally add other app specific claims as needed
            claims.Add(new Claim("UserState", "Active"));

            // create a new token with token helper and add our claim
            // from `Westwind.AspNetCore`  NuGet Package
            var token = service.GenerateToken(username, claims.ToArray());

            return new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            };
        }
    }
}
