using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Commands
{
    public class StockCommand : IRequest<GeneralDTO>
    {
        [Required(ErrorMessage = "ProductName is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "ProductName must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "ProductName can only contain letters.")]
        [DefaultValue("ProductName")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
