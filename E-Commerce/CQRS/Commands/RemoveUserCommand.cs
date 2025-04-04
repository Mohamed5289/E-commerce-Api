using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Commands
{
    public class RemoveUserCommand : IRequest<GeneralDTO>
    {
        public RemoveUserCommand(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
