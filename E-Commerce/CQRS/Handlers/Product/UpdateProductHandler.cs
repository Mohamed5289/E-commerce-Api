using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.ModelHelpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Product
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, GeneralDTO>
    {
        private readonly IdContext idContext;
        private readonly ImageService imageService;
        public UpdateProductHandler(IdContext idContext , ImageService imageService)
        {
            this.idContext = idContext;
            this.imageService = imageService;
        }
        public async Task<GeneralDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var transaction = await idContext.Database.BeginTransactionAsync();

            try
            {
                var product = await idContext.Products.FirstOrDefaultAsync(x => x.Name == request.Name);
                
                if (product is null)
                {
                    return new GeneralDTO
                    {
                        IsSuccess = false,
                        Message = "Product not found"
                    };
                }

                var category = await idContext.Categories.FirstOrDefaultAsync(x => x.Name == request.Category);


                if(request.NewName != null)
                {
                    product.Name = request.NewName;
                }

                if (request.Price != null)
                {
                    product.Price = request.Price.Value;
                }

                if (request.Description != null)
                {
                    product.Description = request.Description;
                }

                if (request.ImageUrl != null)
                {
                    var image = product.ImageUrl.Split("/").Last();
                    imageService.DeleteImage(image, "wwwroot", "image");
                    product.ImageUrl = request.ImageUrl;
                }

                if (request.Category != null) {
                    product.Category = category;
                }

                await idContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new GeneralDTO
                {
                    IsSuccess = true,
                    Message = "Product updated successfully"
                };
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
