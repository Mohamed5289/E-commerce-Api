using E_Commerce.CQRS.Queries;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Stock
{
    public class StocksHandler : IRequestHandler<StocksQuery, List<StockDTO>?>
    {
        private readonly IdContext idContext;

        public StocksHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }

        public async Task<List<StockDTO>?> Handle(StocksQuery request, CancellationToken cancellationToken)
        {
            return await idContext.Stocks
                .Select(s => new StockDTO
                {
                    Product = s.Product!.Name,
                    Quantity = s.Quantity
                })
                .ToListAsync();
        }
    }
}
