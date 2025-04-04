using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Stock
{
    public class AddStockHandler: IRequestHandler<StockCommand, GeneralDTO>
    {
        private readonly IdContext idContext;
        public AddStockHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }
        public async Task<GeneralDTO> Handle(StockCommand request, CancellationToken cancellationToken)
        {
            var transaction = await idContext.Database.BeginTransactionAsync();
            try 
            {
                var product = await idContext.Products.FirstOrDefaultAsync(p => p.Name == request.ProductName);
                if (product == null)
                {
                    return new GeneralDTO
                    {
                        IsSuccess = false,
                        Message = "ProductName not found."
                    };
                }
                var result = await idContext.Stocks.AddAsync(new Models.Stock
                {
                    ProductId = product.Id,
                    Quantity = request.Quantity
                });

                if (result is null || result.State != EntityState.Added)
                {
                    return new GeneralDTO
                    {
                        IsSuccess = false,
                        Message = "Stock not added."
                    };
                }

                await idContext.SaveChangesAsync();
                transaction.Commit();

                return new GeneralDTO
                {
                    IsSuccess = true,
                    Message = "Stock added successfully."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO { Message = ex.Message };
            }
        }
    }
}
