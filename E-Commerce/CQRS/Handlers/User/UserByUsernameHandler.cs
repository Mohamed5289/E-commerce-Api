using E_Commerce.CQRS.Queries;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.User
{
    public class UserByUsernameHandler : IRequestHandler<UserByUsernameQuery, UserDTO?>
    {
        private readonly IdContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UserByUsernameHandler(IdContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserDTO?> Handle(UserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null)
                return null;

            return new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? "email",
                UserName = request.Username,
                PhoneNumber = user.PhoneNumber ?? "phone number",
                IsEmailConfirmed = user.EmailConfirmed
            };
        }
    }
}
