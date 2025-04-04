using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Category
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, GeneralDTO>
    {
        private readonly IdContext _idContext;
        public UpdateCategoryHandler(IdContext idContext)
        {
            _idContext = idContext;
        }
        public async Task<GeneralDTO> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _idContext.Database.BeginTransactionAsync();

            try
            {
                var category = await _idContext.Categories.FirstOrDefaultAsync(x => x.Name == request.Name);

                if (category is null)
                {
                    return new GeneralDTO { IsSuccess = false, Message = "Category not found." };
                }
                category.Name = request.NewName;
                await _idContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new GeneralDTO { IsSuccess = true, Message = "Category updated successfully." };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
