using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.Supplier
{
    public class SupplierHandler : IRequestHandler<SupplierCommand, GeneralDTO>
    {
        private readonly IdContext _context;
        private readonly UserManager<AppUser> _userManager;
        public SupplierHandler(IdContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<GeneralDTO> Handle(SupplierCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Check if the username already exists
                var existingUser = await _userManager.FindByNameAsync(request.Username);

                if (existingUser is null)
                {
                    return new GeneralDTO
                    {
                        Message = "Username is not found"
                    };
                }


                // Create a new supplier
                var supplier = new Models.Supplier
                {
                    // Add other properties as needed
                    AppUserId = existingUser!.Id
                };
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return new GeneralDTO
                {
                    Message = "Supplier created successfully.",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new GeneralDTO
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
