using Backend.API.Models;
using Backend.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddScoped<AuthTokenService>();
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
    var jwtSettings = new JwtSettings();
    builder.Configuration.Bind(key: nameof(JwtSettings), instance: jwtSettings);
    builder.Services.AddAuthentication(i =>
    {
        i.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        i.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        i.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        i.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    })
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = jwtSettings.Issuer,
                       ValidAudience = jwtSettings.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                       ClockSkew = jwtSettings.Expire
                   };
                   options.SaveToken = true;
               });
    builder.Services.AddControllers();
}
var app = builder.Build();
{
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapGet("/", () => "Hello World!");
    app.MapGet("/token/{username}", (string username, AuthTokenService tokenService) => new { access_token = tokenService.GenerateToken(username) });


    app.MapControllers();
}
app.Run();
