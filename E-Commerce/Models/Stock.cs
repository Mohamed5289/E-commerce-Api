using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;

namespace E_Commerce.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
