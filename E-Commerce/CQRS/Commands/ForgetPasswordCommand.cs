using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using E_Commerce.ModelDTOs;

namespace E_Commerce.CQRS.Commands
{
    public class ForgetPasswordCommand : IRequest<Verification>
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9.AdminController%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email format.")]
        [DefaultValue("john.doe@example.com")]
        public string Email { get; set; }
    }
}
