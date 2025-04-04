using E_Commerce.CQRS.Commands;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.Admin
{
    public class RemoveUserHandler : IRequestHandler<RemoveUserCommand, GeneralDTO>
    {
        private readonly UserManager<AppUser> userManager;

        public RemoveUserHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<GeneralDTO> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.Username);

            if (user is null)
                return new GeneralDTO { Message = "Username is not found " };

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return new GeneralDTO { Message = errors };

            }

            return new GeneralDTO
            {
                Message = $"This User {request.Username} is Deleted",
                IsSuccess = true
            };
        }
    }
}
