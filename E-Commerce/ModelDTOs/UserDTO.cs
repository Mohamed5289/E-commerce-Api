namespace E_Commerce.ModelDTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public bool IsEmailConfirmed { get; set; }
    }
}
