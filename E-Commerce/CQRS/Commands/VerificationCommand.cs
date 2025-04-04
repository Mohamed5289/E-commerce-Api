using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using E_Commerce.ModelDTOs;

namespace E_Commerce.CQRS.Commands
{
    public class VerificationCommand : IRequest<AuthenticationResponse>
    {

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9.AdminController%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Invalid email format.")]
        [DefaultValue("john.doe@example.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Verification code is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Verification code must be exactly 6 digits.")]
        [DefaultValue("123456")]
        public string VerificationCode { get; set; }

    }
}
