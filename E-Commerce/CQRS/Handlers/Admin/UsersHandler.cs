using E_Commerce.CQRS.Queries;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.Admin
{
    public class UsersHandler : IRequestHandler<UsersQuery, List<UserDTO>?>
    {
        private readonly IdContext context;

        public UsersHandler(IdContext context)
        {
            this.context = context;
        }
        public async Task<List<UserDTO>?> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var users = await context.Users
                .Select(u => new UserDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email!,
                    UserName = u.UserName!,
                    PhoneNumber = u.PhoneNumber!,
                    IsEmailConfirmed = u.EmailConfirmed

                })
                .ToListAsync(cancellationToken);
            return users;
        }
    }
}
