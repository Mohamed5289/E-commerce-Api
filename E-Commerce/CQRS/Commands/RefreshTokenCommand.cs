using E_Commerce.ModelDTOs;
using MediatR;

namespace E_Commerce.CQRS.Commands
{
    public record RefreshTokenCommand(string Token) : IRequest<AuthenticationResponse>;
}
