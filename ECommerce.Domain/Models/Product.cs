


namespace ECommerce.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Status { get; set; }  = default!;
        public decimal BasePrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Merchant Merchant { get; set; } = null!;
        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}
