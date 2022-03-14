using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.DTOs
{
    public class ProductsPaginationDTO
    {
        public List<Product> Products { get; set; } = new();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
}
