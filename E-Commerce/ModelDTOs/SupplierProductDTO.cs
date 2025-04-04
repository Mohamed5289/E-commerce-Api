namespace E_Commerce.ModelDTOs
{
    public class SupplierProductDTO
    {
        public string Message { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public List<SupplierProductDetails> Details { get; set; } = new List<SupplierProductDetails>();
    }
}
