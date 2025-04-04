using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Category
{
    public class SeedCategoriesHandler : IRequestHandler<CategoryCommand, GeneralDTO>
    {
        private readonly IdContext idContext;

        public SeedCategoriesHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }
        public async Task<GeneralDTO> Handle(CategoryCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await idContext.Database.BeginTransactionAsync();
            try
            {
                var isExist = await idContext.Categories.AnyAsync(c => c.Name == request.Name);

                if (isExist)
                {
                    await transaction.RollbackAsync();
                    return new GeneralDTO { IsSuccess = false, Message = "Category already exists." };
                }


                var result = await idContext.Categories.AddAsync(new Models.Category { Name = request.Name });

                if (result.State != EntityState.Added)
                {
                    await transaction.RollbackAsync();
                    return new GeneralDTO { IsSuccess = false, Message = "Category not added." };
                }

                await idContext.SaveChangesAsync();
                transaction.Commit();
                return new GeneralDTO { IsSuccess = true, Message = "Category added successfully." };

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
