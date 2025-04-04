using System.Text.Json.Serialization;

namespace E_Commerce.ModelDTOs
{
    public class Verification
    {
        public string Message { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;

        [JsonIgnore]
        public string Email { get; set;} = string.Empty;
    }
}
