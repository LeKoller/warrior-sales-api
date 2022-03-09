using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WarriorSalesAPI.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; }
        public DateTime Creation { get; set; } = DateTime.Now;
        public DateTime? Delivery { get; set; } = null;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public ICollection<SaleItem> Items { get; set; }
        public Team Team { get; set; }
    }
}
