

namespace ECommerce.Domain.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ApplicationUser AppUser { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
