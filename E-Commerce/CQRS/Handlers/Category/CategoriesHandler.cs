using E_Commerce.CQRS.Queries;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Category
{
    public class CategoriesHandler : IRequestHandler<CategoriesQuery, List<CategoryDTO>?>
    {
        private readonly IdContext idContext;

        public CategoriesHandler(IdContext idContext)
        {
            this.idContext = idContext;
        }
        public async Task<List<CategoryDTO>?> Handle(CategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await idContext.Categories.AsTracking().Select(x => new CategoryDTO { Name = x.Name}).ToListAsync();
            return categories;
        }
    }
}
