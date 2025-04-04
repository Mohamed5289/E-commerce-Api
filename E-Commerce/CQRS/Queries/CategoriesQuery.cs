using E_Commerce.ModelDTOs;
using MediatR;

namespace E_Commerce.CQRS.Queries
{
    public record CategoriesQuery : IRequest<List<CategoryDTO>>;
}
