namespace E_Commerce.ModelDTOs
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Message { get; set; }
        public bool Succeeded { get; set; } = false;
    }
}
