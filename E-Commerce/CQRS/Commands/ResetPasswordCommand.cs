using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using E_Commerce.ModelDTOs;

namespace E_Commerce.CQRS.Commands
{
    public class ResetPasswordCommand  : IRequest<ResetPassword>
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9.AdminController%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Invalid email format.")]
        [DefaultValue("john.doe@example.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DefaultValue("Password123!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Verification code is required.")]
        public string Code { get; set; }
    }
}
