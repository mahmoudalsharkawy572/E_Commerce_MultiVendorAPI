using MediatR;

namespace ECommerce.Application.ProductVariants.Commands.UpdateProductVariant
{
    public class UpdateProductVariantCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string SKU { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal? PriceOverride { get; set; }
        public bool IsActive { get; set; }
    }
}
