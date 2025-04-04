using E_Commerce.ModelDTOs;
using MediatR;

namespace E_Commerce.CQRS.Commands
{
    public class DeleteOrderCommand(int Id) : IRequest<GeneralDTO>
    {
        public int Id { get; set; } = Id;
    }
}
