using E_Commerce.CQRS.Commands;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.User
{
    public class ForgetPasswordHandler : IRequestHandler<ForgetPasswordCommand, Verification>
    {
        private readonly UserManager<AppUser> userManager;

        public ForgetPasswordHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Verification> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return new Verification { Message = "Email not found." };

            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            return new Verification
            {
                Message = "Email is Valid ",
                VerificationCode = code,
                Succeeded = true
            };


        }
    }
}
