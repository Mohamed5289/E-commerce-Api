using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Order
{
    public class OrderHandler : IRequestHandler<OrderCommand, GeneralDTO>
    {
        private readonly IdContext idContext;
        private readonly UserManager<AppUser> userManager;
        public OrderHandler(IdContext idContext, UserManager<AppUser> userManager)
        {
            this.idContext = idContext;
            this.userManager = userManager;
        }
        public async Task<GeneralDTO> Handle(OrderCommand request, CancellationToken cancellationToken)
        {
            var transaction = await idContext.Database.BeginTransactionAsync();
            try
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return new GeneralDTO
                    {
                        IsSuccess = false,
                        Message = "User not found"
                    };
                }

                // Create order
                var order = new Models.Order
                {
                    AppUser = user,
                    OrderItems = new List<OrderItem>(),
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    TotalPrice = 0
                };


                foreach (var item in request.Items)
                {
                    var product = await idContext.Products.FirstOrDefaultAsync(p => p.Name == item.ProductName);

                    if (product == null)
                    {
                        return new GeneralDTO
                        {
                            IsSuccess = false,
                            Message = $" Product : {item.ProductName} not found "
                        };
                    }


                    if (product.Stock is null || product.Stock.Quantity < item.Quantity)
                    {
                        return new GeneralDTO
                        {
                            IsSuccess = false,
                            Message = $" Product : {item.ProductName} , Quantity {item.Quantity} is out of stock "
                        };
                    }

                    var orderItem = new OrderItem
                    {
                        Product = product,
                        Quantity = item.Quantity,
                        Order = order
                    };

                    orderItem.Price = product.Price * item.Quantity;
                    order.OrderItems.Add(orderItem);
                    order.TotalPrice += orderItem.Price;
                }

                idContext.Orders.Add(order);
                await idContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return new GeneralDTO { IsSuccess = true, Message = "Order created successfully" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO { IsSuccess = false, Message = ex.Message };
            }

        }
    }
}
