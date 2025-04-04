using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Supplier
{
    public class SuppliersHandler : IRequestHandler<SuppliersCommand, List<SupplierDTO>>
    {
        private readonly IdContext _context;

        public SuppliersHandler(IdContext context)
        {
            _context = context;
        }
        public async Task<List<SupplierDTO>> Handle(SuppliersCommand request, CancellationToken cancellationToken)
        {
            var suppliers = await _context.Suppliers
              
                .Select(s => new SupplierDTO
                {
                    Id = s.Id,
                    Username = s.AppUser!.UserName!
                })
                .ToListAsync(cancellationToken);

            return suppliers;
        }
    }
}
