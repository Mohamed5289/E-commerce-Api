using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Commands
{
    public class DeleteProductCommand : IRequest<GeneralDTO>
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]
        [DefaultValue("ProductName")]
        public string Name { get; set; }
    }
}
