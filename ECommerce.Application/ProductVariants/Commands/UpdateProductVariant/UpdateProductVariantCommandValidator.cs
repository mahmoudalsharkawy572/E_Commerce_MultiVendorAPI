using FluentValidation;

namespace ECommerce.Application.ProductVariants.Commands.UpdateProductVariant
{
    public class UpdateProductVariantCommandValidator : AbstractValidator<UpdateProductVariantCommand>
    {
        public UpdateProductVariantCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Variant ID must be greater than 0");

            RuleFor(x => x.SKU)
                .NotEmpty()
                .WithMessage("SKU is required")
                .MaximumLength(100)
                .WithMessage("SKU cannot exceed 100 characters");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity cannot be negative");

            RuleFor(x => x.PriceOverride)
                .GreaterThan(0)
                .When(x => x.PriceOverride.HasValue)
                .WithMessage("Price override must be greater than 0");
        }
    }
}
