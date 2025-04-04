using E_Commerce.CQRS.Commands;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.Order
{
    public class UserOrdersHandler : IRequestHandler<UserOrdersCommand, List<OrderDTO>?>
    {
        private readonly UserManager<AppUser> _userManager;
        public UserOrdersHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<OrderDTO>?> Handle(UserOrdersCommand request, CancellationToken cancellationToken)
        {
            var user =  await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return null;
            }

            return user.Orders!.Select(o => new OrderDTO
            {
                Id = o.Id,
                Username = request.Username,
                Status = o.Status,
                Total = o.TotalPrice,
                Items = o.OrderItems!.Select(oi => new OrderItemDTO
                {
                    ProductName = oi.Product!.Name!,
                    Quantity = oi.Quantity
                }).ToList()
            }).ToList();

        }
    }
}
