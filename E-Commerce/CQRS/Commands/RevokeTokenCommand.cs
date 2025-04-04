using MediatR;

namespace E_Commerce.CQRS.Commands
{
    public record RevokeTokenCommand(string Token) : IRequest<bool>;
}
