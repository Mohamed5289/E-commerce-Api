using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using E_Commerce.ModelDTOs;

namespace E_Commerce.CQRS.Commands
{
    public class RegisterCommand : IRequest<Verification>
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        [DefaultValue("John")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
        [DefaultValue("Doe")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        [DefaultValue("johndoe123")]

        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9.AdminController%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Invalid email format.")]
        [DefaultValue("john.doe@example.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "City must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        [DefaultValue("City")]

        public string City { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Street must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Street can only contain letters.")]
        [DefaultValue("Street")]

        public string Street { get; set; }

        [Required(ErrorMessage = "Governorate is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Governorate must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Governorate can only contain letters.")]
        [DefaultValue("John")]

        public string Governorate { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DefaultValue("Password123!")]
        public string Password { get; set; }
    }
}
