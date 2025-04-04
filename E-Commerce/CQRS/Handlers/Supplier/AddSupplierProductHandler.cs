using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Supplier
{
    public class AddSupplierProductHandler : IRequestHandler<SupplierProductCommand, GeneralDTO>
    {
        private readonly IdContext idContext;
        private readonly UserManager<AppUser> userManager;

        public AddSupplierProductHandler(IdContext idContext, UserManager<AppUser> userManager)
        {
            this.idContext = idContext;
            this.userManager = userManager;
        }
        public async Task<GeneralDTO> Handle(SupplierProductCommand request, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByNameAsync(request.Username);
            if (user is null)
            {
                return new GeneralDTO
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }

            var supplier = await idContext.Suppliers
                .FirstOrDefaultAsync(s => s.AppUserId == user.Id, cancellationToken);

            if (supplier is null)
            {
                return new GeneralDTO
                {
                    IsSuccess = false,
                    Message = "Supplier not found."
                };
            }

            var product = await idContext.Products
                .FirstOrDefaultAsync(p => p.Name == request.ProductName, cancellationToken);

            if (product is null)
            {
                return new GeneralDTO
                {
                    IsSuccess = false,
                    Message = "Product not found."
                };

            }

            var supplierProduct = new SupplierProduct
            {
                SupplierId = supplier.Id,
                ProductId = product.Id,
                Quantity = request.Quantity,
                Price = product.Price * request.Quantity,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            await idContext.SupplierProducts.AddAsync(supplierProduct, cancellationToken);
            await idContext.SaveChangesAsync();

            return new GeneralDTO
            {
                IsSuccess = true,
                Message = "Supplier product added successfully."
            };
        }
    }
}
