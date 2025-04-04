using E_Commerce.CQRS.Queries;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Admin
{
    public class RolesHandler : IRequestHandler<RoleQuery, List<RoleDTO>>
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesHandler(RoleManager<IdentityRole> roleManager) 
        {
            this.roleManager = roleManager;
        }

        public async Task<List<RoleDTO>> Handle(RoleQuery request, CancellationToken cancellationToken)
        {
             var roles =await roleManager.Roles
                .AsNoTracking()
                .Select(x => new RoleDTO { RoleName = x.Name! })
                .ToListAsync(cancellationToken);

            return roles;
        }
    }
}
