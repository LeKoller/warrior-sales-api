namespace WarriorSalesAPI.DTOs
{
    public class AddOrderDTO
    {
        public string Address { get; set; } = String.Empty;
        public List<CartItem> Cart { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
