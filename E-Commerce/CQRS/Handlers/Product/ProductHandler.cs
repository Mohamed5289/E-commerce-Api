using E_Commerce.CQRS.Queries;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Product
{
    public class ProductHandler : IRequestHandler<ProductQuery, ProductDTO?>
    {
        private readonly IdContext idContext;

        public ProductHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }

        public async Task<ProductDTO?> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            var product = await idContext.Products.FirstOrDefaultAsync(x => x.Name == request.Name);
            if (product == null)
            {
                return null;
            }

            return new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                Category = product.Category!.Name,
                ImageUrl = product.ImageUrl
            };
        }
    }
}
