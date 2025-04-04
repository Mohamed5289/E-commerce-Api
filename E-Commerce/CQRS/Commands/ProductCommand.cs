using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.CQRS.Commands
{
    public class ProductCommand : IRequest<GeneralDTO>
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, spaces ")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters.")]
        [RegularExpression(@"^[A-Za-z0-9 ]+$", ErrorMessage = "Name can only contain letters, numbers, spaces ")]
        public string Description { get; set; }

        [Url(ErrorMessage = "ImageUrl must be a valid URL.")]
        [RegularExpression(@"^https?:\/\/.*\.(?:png|jpg|jpeg|gif)$", ErrorMessage = "ImageUrl must be a valid image URL (jpg, png, gif).")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(100, ErrorMessage = "Category can't be longer than 100 characters.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Category can only contain letters and spaces.")]
        public string Category { get; set; }

    }
}
