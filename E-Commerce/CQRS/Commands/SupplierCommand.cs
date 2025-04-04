using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Commands
{
    public class SupplierCommand : IRequest<GeneralDTO>
    {

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        [DefaultValue("johndoe123")]

        public string Username { get; set; }
    }
}
