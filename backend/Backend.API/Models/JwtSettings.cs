namespace Backend.API.Models
{
    public class JwtSettings
    {
        public string SecretKey { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int MinutesToExpiration { get; set; }
        public TimeSpan Expire => TimeSpan.FromMinutes(MinutesToExpiration);
    }
}
