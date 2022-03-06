using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WarriorSalesAPI.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime Creation { get; set; } = DateTime.Now;
        public DateTime? Delivery { get; set; } = null;
        [Required]
        public string Address { get; set; } = string.Empty;

        public List<Product> Products { get; set; }
    }
}
