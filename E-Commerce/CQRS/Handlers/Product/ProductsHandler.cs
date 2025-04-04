using E_Commerce.CQRS.Queries;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Product
{
    public class ProductsHandler : IRequestHandler<ProductsQuery, List<ProductDTO>?>
    {
        private readonly IdContext idContext;
        public ProductsHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }
        public async Task<List<ProductDTO>?> Handle(ProductsQuery request, CancellationToken cancellationToken)
        {
            return await idContext.Products.Select(p => new ProductDTO
            {
                Name = p.Name,
                Price = p.Price,
                Category = p.Category!.Name,
                ImageUrl = p.ImageUrl

            }).ToListAsync();
        }
    }
}
