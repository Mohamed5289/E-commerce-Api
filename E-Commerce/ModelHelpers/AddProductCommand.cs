using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

public class AddProductCommand 
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]
    [DefaultValue("ProductName")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10,000.")]
    [DefaultValue(1.00)]
    public decimal Price { get; set; } = 1.00m;

    [Required(ErrorMessage = "Description required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 50 characters.")]
    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Description can only contain letters.")]
    [DefaultValue("Description")]
    public string Description { get; set; }


    [Required(ErrorMessage = "Image is required.")]
    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 50 characters.")]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Category name can only contain letters.")]
    [DefaultValue("Category")]
    public string Category { get; set; }

}
