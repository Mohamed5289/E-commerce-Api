using System.Text.Json.Serialization;

namespace E_Commerce.ModelDTOs
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        public List<string> Roles { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiration { get; set; }
    }
}
