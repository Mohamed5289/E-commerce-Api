using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace E_Commerce.CQRS.Commands
{
    public class UpdateProductCommand : IRequest<GeneralDTO>
    {
        public string? Name { get; set; }
        public string? NewName { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
    }
}
