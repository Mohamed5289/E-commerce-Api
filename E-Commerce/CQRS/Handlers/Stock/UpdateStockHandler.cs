using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Stock
{
    public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, GeneralDTO>
    {
        private readonly IdContext idContext;
        public UpdateStockHandler(IdContext idContext ) 
        {
            this.idContext = idContext;
        }
        public async Task<GeneralDTO> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var transaction = await idContext.Database.BeginTransactionAsync();
            try
            {
                var product = await idContext.Products.FirstOrDefaultAsync(x => x.Name == request.ProductName);
                if (product == null)
                {

                    return new GeneralDTO { Message = "Product not found." };
                }

                var stock = await idContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == product.Id);
                if (stock == null)
                {
                    return new GeneralDTO { Message = "Stock not found." };
                }

                stock.Quantity = request.Quantity;

                var result = idContext.Stocks.Update(stock);

                if (result is null || result.State != EntityState.Modified)
                {
                    return new GeneralDTO { Message = "Stock not updated." };
                }
                await idContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new GeneralDTO { Message = "Stock updated successfully.", IsSuccess = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO { Message = ex.Message };
            }

        }
    }
}
