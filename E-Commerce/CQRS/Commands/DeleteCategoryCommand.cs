using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Commands
{
    public class DeleteCategoryCommand : IRequest<GeneralDTO>
    {
        public DeleteCategoryCommand(string categoryName)
        {
            CategoryName = categoryName;
        }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Category name can only contain letters.")]
        [DefaultValue("Category")]
        public string CategoryName { get; set; }
    }
}
