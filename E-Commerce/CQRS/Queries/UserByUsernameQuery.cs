using E_Commerce.ModelDTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace E_Commerce.CQRS.Queries
{
    public class UserByUsernameQuery : IRequest<UserDTO>
    {
        public UserByUsernameQuery() { }
        public UserByUsernameQuery(string username)
        {
            Username = username;
        }
        public string Username { get; set; } =  string.Empty;

    }
}
