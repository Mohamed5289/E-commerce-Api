using E_Commerce.ModelDTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;

namespace E_Commerce.CQRS.Commands
{
    public class CategoryCommand : IRequest<GeneralDTO>
    {

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Category must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Category can only contain letters.")]
        [DefaultValue("Category")]
        public string Name { get; set; }
    }
}
