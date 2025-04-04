using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Category
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, GeneralDTO>
    {
        private readonly IdContext idContext;

        public DeleteCategoryHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }
        public async Task<GeneralDTO> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await idContext.Database.BeginTransactionAsync();

            try
            {
                var category = await idContext.Categories.FirstOrDefaultAsync(x => x.Name == request.CategoryName);

                if (category is null)
                {
                    await transaction.RollbackAsync();
                    return new GeneralDTO { IsSuccess = false, Message = "Category not found" };
                }

                var result = idContext.Categories.Remove(category);
                if (result.State != EntityState.Deleted)
                {
                    await transaction.RollbackAsync();
                    return new GeneralDTO { IsSuccess = false, Message = "Category not deleted" };
                }

                await idContext.SaveChangesAsync();
                transaction.Commit();
                return new GeneralDTO { IsSuccess = true, Message = "Category deleted successfully" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO { IsSuccess = false, Message = ex.Message };
            }
        }  
    }
}
