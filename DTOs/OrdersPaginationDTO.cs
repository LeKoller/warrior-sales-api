using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.DTOs
{
    public class OrdersPaginationDTO
    {
        public List<OrderListDTO> Orders { get; set; } = new List<OrderListDTO>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }

    public class OrderListDTO
    {
        public string Address { get; set; } = string.Empty;
        public DateTime Creation { get; set; } = DateTime.Now;
        public DateTime? Delivery { get; set; } = null;
        public Team Team { get; set; }
    }
}
