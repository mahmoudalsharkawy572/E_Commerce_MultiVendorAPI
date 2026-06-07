
namespace ECommerce.Domain.Models
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;

        // Navigation
        public Product Product { get; set; } = null!;
        public ICollection<AttributeValue> Values { get; set; } = new List<AttributeValue>();
    }
}
