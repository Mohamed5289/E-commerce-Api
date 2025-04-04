using E_Commerce.CQRS.Queries;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Order
{
    public class OrdersHandler : IRequestHandler<OrderQuery, List<OrderDTO>?>
    {
        private readonly IdContext _idContext;
        public OrdersHandler(IdContext idContext)
        {
            _idContext = idContext;
        }

        public async Task<List<OrderDTO>?> Handle(OrderQuery request, CancellationToken cancellationToken)
        {
            return await _idContext.Orders
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    Username = o.AppUser!.UserName!,
                    Status = o.Status,
                    Total = o.TotalPrice,
                    Items = o.OrderItems!.Select(oi => new OrderItemDTO
                    {
                        ProductName = oi.Product!.Name!,
                        Quantity = oi.Quantity
                    }).ToList()
                }).ToListAsync();
        }
    }
}
