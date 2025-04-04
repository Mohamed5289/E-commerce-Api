using E_Commerce.CQRS.Commands;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.Admin
{
    public class SeedRolesHandler : IRequestHandler<RoleCommand, GeneralDTO>
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public SeedRolesHandler(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<GeneralDTO> Handle(RoleCommand request, CancellationToken cancellationToken)
        {

            var result = await roleManager.CreateAsync(new IdentityRole(request.RoleName));

            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return new GeneralDTO { Message = errors };

            }

            return new GeneralDTO
            {
                Message = $"Role '{request.RoleName}' has been created successfully.",
                IsSuccess = true
            };
        }
    }
}
