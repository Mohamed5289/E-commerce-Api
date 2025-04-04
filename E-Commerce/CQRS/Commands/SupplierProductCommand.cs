using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.CQRS.Commands
{
    public class SupplierProductCommand : IRequest<GeneralDTO>
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, spaces ")]
        [DefaultValue("Product")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        [DefaultValue("johndoe123")]

        public string Username { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        [DefaultValue(1)]
        public int Quantity { get; set; }

    }
}
