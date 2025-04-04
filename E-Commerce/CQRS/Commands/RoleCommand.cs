using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Commands
{
    public class RoleCommand : IRequest<GeneralDTO>
    {
        public RoleCommand() { }

        public RoleCommand(string roleName)
        {
            this.RoleName = roleName;
        }

        [Required(ErrorMessage = " Role is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Role must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Role can only contain letters.")]
        [DefaultValue("User")]
        public string RoleName { get; set; } = string.Empty;
    }

}
