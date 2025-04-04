using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.CQRS.Commands
{
    public class CreatePaymentCommand : IRequest<GeneralDTO>
    {
        // Ensure Currency is not null and must be a valid currency code
        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters.")]
        [DefaultValue("USD")]
        public string Currency { get; set; } = "USD";  // Default to "USD"

        // Ensure OrderId is not null or empty
        [Required(ErrorMessage = "OrderId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderId must be a positive integer.")]
        [DefaultValue(1)]
        public int OrderId { get; set; } 
    }
}
