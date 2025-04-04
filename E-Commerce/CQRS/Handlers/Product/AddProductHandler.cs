using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Product
{
    public class AddProductHandler : IRequestHandler<ProductCommand, GeneralDTO>
    {
        private readonly IdContext idContext;

        public AddProductHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }
        public async Task<GeneralDTO> Handle(ProductCommand request,CancellationToken cancellationToken)
        {
            using var transaction = await idContext.Database.BeginTransactionAsync();

            try
            {
                var category = idContext.Categories.FirstOrDefault(c => c.Name == request.Category);
                if (category == null)
                {
                    return new GeneralDTO { IsSuccess = false, Message = "Category not found." };
                }

                var productExist = idContext.Products.FirstOrDefault(p => p.Name == request.Name);

                if(productExist != null)
                {

                    return new GeneralDTO { IsSuccess = false, Message = "ProductName already exist." };
                }

                var product = new Models.Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    CategoryId = category.Id
                };

                var result =  await idContext.Products.AddAsync(product);

                if(result.State != EntityState.Added)
                {
                    await transaction.RollbackAsync();
                    return new GeneralDTO { IsSuccess = false, Message = "ProductName not added." };
                }

                await idContext.SaveChangesAsync();
                transaction.Commit();
                return new GeneralDTO { IsSuccess = true, Message = "ProductName added successfully." };

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralDTO
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }
    }
}
