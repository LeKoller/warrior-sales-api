using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WarriorSalesAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public float Price { get; set; } = 0;
        [Required]
        public string Category { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;
        [Required]
        public int Stock { get; set; } = 0;
        [JsonIgnore]
        public ICollection<SaleItem>? SoldItems { get; set; }
    }
}
