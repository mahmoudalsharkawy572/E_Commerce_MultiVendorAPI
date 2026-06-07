using FluentValidation;

namespace ECommerce.Application.VariantAttributeValues.Commands.CreateVariantAttributeValue
{
    public class CreateVariantAttributeValueCommandValidator : AbstractValidator<CreateVariantAttributeValueCommand>
    {
        public CreateVariantAttributeValueCommandValidator()
        {
            RuleFor(x => x.ProductVariantId)
                .GreaterThan(0)
                .WithMessage("Product Variant ID must be greater than 0");

            RuleFor(x => x.AttributeValueId)
                .GreaterThan(0)
                .WithMessage("Attribute Value ID must be greater than 0");
        }
    }
}
