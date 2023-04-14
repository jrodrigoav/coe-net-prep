using Backend.API.Models;
using Backend.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddScoped<AuthTokenService>();
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
    var jwtSettings = new JwtSettings();
    builder.Configuration.Bind(key: nameof(JwtSettings), instance: jwtSettings);

    builder.Services.AddAuthentication(auth =>
     {
         auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     })
     .AddJwtBearer(options =>
     {
         options.SaveToken = true;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidIssuer = jwtSettings.Issuer,
             ValidateAudience = true,
             ValidAudience = jwtSettings.Audience,
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
         };
     });
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
    builder.Services.AddControllers();
}
var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.MapGet("/", () => "Ghetto JWT / Training");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}
app.Run();
