using E_Commerce.ModelDTOs;
using MediatR;

namespace E_Commerce.CQRS.Queries
{
    public record OrderQuery : IRequest<List<OrderDTO>>;
}
