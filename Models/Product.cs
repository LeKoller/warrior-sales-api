using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarriorSalesAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public float Price { get; set; } = 0;
        [Required]
        public string Type { get; set; } = string.Empty;
        public string? Description { get; set; } = String.Empty;
        [Required]
        public int Stock { get; set; } = 0;

        public List<Order> Orders { get; set; }
    }
}
