using E_Commerce.ModelHelpers;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Address Address { get; set; }
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual Supplier? Supplier { get; set; }
    }
}
