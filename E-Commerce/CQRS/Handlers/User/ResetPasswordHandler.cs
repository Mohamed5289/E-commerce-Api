using E_Commerce.CQRS.Commands;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.User
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, ResetPassword>
    {

        private readonly UserManager<AppUser> userManager;
        public ResetPasswordHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<ResetPassword> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new ResetPassword { Message = "Email is not found" };
            }

            var result = await userManager.ResetPasswordAsync(user, request.Code, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return new ResetPassword { Message = errors };
            }

            return new ResetPassword
            {
                Succeeded = true,
                Message = "Password is Changed",
                Email = request.Email
            };
        }
    }
}
