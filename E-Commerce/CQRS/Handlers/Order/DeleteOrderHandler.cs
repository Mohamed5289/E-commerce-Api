using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Order
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, GeneralDTO>
    {
        private readonly IdContext idContext;
        public DeleteOrderHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }
        public async Task<GeneralDTO> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        { 
            var transaction = await idContext.Database.BeginTransactionAsync();

            try
            {
                var order = await idContext.Orders.FindAsync(request.Id);
                if (order == null)
                {
                    return new GeneralDTO
                    {
                        IsSuccess = false,
                        Message = "Order not found"
                    };
                }

                if(order.Status != "Pending")
                {
                    return new GeneralDTO
                    {
                        IsSuccess = false,
                        Message = $"Order already {order.Status}"
                    };
                }

                var result = idContext.Orders.Remove(order);

                if (result == null || result.State != EntityState.Deleted)
                {
                    return new GeneralDTO
                    {
                        IsSuccess = false,
                        Message = "Order not deleted"
                    };
                }

                await idContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return new GeneralDTO
                {
                    IsSuccess = true,
                    Message = "Order deleted"
                };

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
    }
}
