using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Supplier
{
    public class SupplierByUsernameHandler : IRequestHandler<SupplierByUsernameCommand, SupplierProductDTO>
    {
        private readonly IdContext _context;
        private readonly UserManager<AppUser> _userManager;
        public SupplierByUsernameHandler(IdContext context , UserManager<AppUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<SupplierProductDTO> Handle(SupplierByUsernameCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
               return new SupplierProductDTO
               {
                   Message = "User not found"
               };
            }

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.AppUser!.UserName == request.Username, cancellationToken);
            
            if (supplier == null)
            {
                return new SupplierProductDTO
                {
                    Message = "Supplier not found"
                };
                
            }

            var supplierProducts = new SupplierProductDTO
            {
                Id = supplier.Id,
                Username = request.Username,

                Details = supplier.SupplierProducts.Select(sp => new SupplierProductDetails
                {
                    ProductName = sp.Product!.Name,
                    Price = sp.Price,
                    Quantity = sp.Quantity,
                    CreationDate = sp.CreatedAt
                }).ToList()
            };

            return supplierProducts;
        }
    }
}
