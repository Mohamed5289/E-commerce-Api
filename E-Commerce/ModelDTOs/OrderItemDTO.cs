using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.ModelDTOs
{
    public class OrderItemDTO
    {
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Product Name must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Product Name can only contain letters.")]
        [DefaultValue("item")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
