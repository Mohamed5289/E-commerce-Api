using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace E_Commerce.ModelHelpers
{
    public class UpdateProduct
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]
        [DefaultValue("ProductName")]
        public string Name { get; set; }


        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]
        [DefaultValue("ProductName")]
        public string? NameNew { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10,000.")]
        [DefaultValue(0.01)]
        public decimal? Price { get; set; }


        [StringLength(50, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Description can only contain letters.")]
        [DefaultValue("Description")]
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Category name can only contain letters.")]
        [DefaultValue("Category")]
        public string? Category { get; set; }
    }
}
