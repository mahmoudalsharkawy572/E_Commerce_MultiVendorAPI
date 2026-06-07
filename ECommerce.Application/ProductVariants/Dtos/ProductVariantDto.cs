namespace ECommerce.Application.ProductVariants.Dtos
{
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal? PriceOverride { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
