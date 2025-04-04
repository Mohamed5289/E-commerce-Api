using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelHelpers;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.User
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, bool>
    {
        private IdContext _context;
        private Token token;
        private UserManager<AppUser> userManager;

        public RevokeTokenHandler(IdContext context, Token token, UserManager<AppUser> userManager)
        {
            _context = context;
            this.token = token;
            this.userManager = userManager;
        }
        public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == request.Token));

            if (user is null) return false;

            var refreshToken = user.RefreshTokens!.SingleOrDefault(t => t.Token == request.Token)!;

            if (!refreshToken.IsActive) return false;

            refreshToken.Revoked = DateTime.UtcNow;

            await userManager.UpdateAsync(user);

            return true;
        }
    }
}
