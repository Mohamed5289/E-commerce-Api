using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.ModelHelpers;
using E_Commerce.Models;
using MailKit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace E_Commerce.CQRS.Handlers.User
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Verification>
    {
        private readonly IdContext _context;
        private readonly Token token;
        private readonly UserManager<AppUser> userManager;

        public RegisterHandler(IdContext context, Token token, UserManager<AppUser> userManager)
        {
            _context = context;
            this.token = token;
            this.userManager = userManager;
        }
        public async Task<Verification> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new AppUser
                {
                    UserName = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    RefreshTokens = new List<RefreshToken>(),
                    Address = new Address
                    {
                        City = request.City,
                        Street = request.Street,
                        Governorate = request.Governorate
                    }

                };

                var result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return new Verification { Message = string.Join(" ", errors) };
                }

                var verificationCode = await userManager.GenerateEmailConfirmationTokenAsync(user);

                await transaction.CommitAsync(cancellationToken);


                return new Verification { Succeeded = true, Message = "Please Confirm your email" ,  VerificationCode = verificationCode , Email = user.Email};

            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                return new Verification { Message = ex.Message };
            }
        }

    }

}
