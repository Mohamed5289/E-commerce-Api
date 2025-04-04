using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Commands
{
    public class RemoveRoleCommand : IRequest<GeneralDTO>
    {

        [Required(ErrorMessage = "RoleName is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "RoleName must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "RoleName can only contain letters.")]
        [DefaultValue("RoleName")]
        public string RoleName { get; set; }
    }
}
