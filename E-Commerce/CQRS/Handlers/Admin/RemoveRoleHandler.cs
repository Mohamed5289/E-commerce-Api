using E_Commerce.CQRS.Commands;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.Admin
{
    public class RemoveRoleHandler : IRequestHandler<RemoveRoleCommand, GeneralDTO>
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RemoveRoleHandler(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<GeneralDTO> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var roleExist = await roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExist)
            {
                return new GeneralDTO {Message = "Role does not exist." };
            }

            var role = await roleManager.FindByNameAsync(request.RoleName);
            await roleManager.DeleteAsync(role!);
            return new GeneralDTO { IsSuccess = true, Message = "Role removed successfully." };
        }
    }
}
