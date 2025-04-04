using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Commands
{
    public class DeleteStockCommand : IRequest<GeneralDTO>
    {

        [Required(ErrorMessage = "ProductName name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "ProductName name must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "ProductName name can only contain letters.")]
        [DefaultValue("ProductName")]
        public string ProductName { get; set; }
    }
}
