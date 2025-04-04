using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Stock
{
    public class DeleteStockHandler : IRequestHandler<DeleteStockCommand, GeneralDTO>
    {
        private readonly IdContext idContext;
        public DeleteStockHandler(IdContext idContext) 
        {
            this.idContext = idContext;
        }
        public async Task<GeneralDTO> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
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

                var result = idContext.Stocks.Remove(stock);

                if (result is null || result.State != EntityState.Deleted)
                {
                    return new GeneralDTO { Message = "Stock not deleted." };
                }

                await idContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new GeneralDTO { Message = "Stock deleted successfully.", IsSuccess = true };
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO { Message = ex.Message };
            }

        }   
            
    }
}
