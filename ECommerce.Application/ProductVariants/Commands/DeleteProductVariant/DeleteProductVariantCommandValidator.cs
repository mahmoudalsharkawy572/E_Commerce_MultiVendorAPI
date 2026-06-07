using FluentValidation;

namespace ECommerce.Application.ProductVariants.Commands.DeleteProductVariant
{
    public class DeleteProductVariantCommandValidator : AbstractValidator<DeleteProductVariantCommand>
    {
        public DeleteProductVariantCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Variant ID must be greater than 0");
        }
    }
}
