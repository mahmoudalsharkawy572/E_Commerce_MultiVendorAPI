using MediatR;

namespace ECommerce.Application.ProductVariants.Commands.CreateProductVariant
{
    public class CreateProductVariantCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public string SKU { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal? PriceOverride { get; set; }
        public bool IsActive { get; set; }
    }
}
