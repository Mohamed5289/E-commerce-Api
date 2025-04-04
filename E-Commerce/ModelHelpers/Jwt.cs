namespace E_Commerce.ModelHelpers
{
    public class Jwt
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpireOfDay { get; set; }
    }
}
