using FluentValidation;

namespace ECommerce.Application.VariantAttributeValues.Commands.DeleteVariantAttributeValue
{
    public class DeleteVariantAttributeValueCommandValidator : AbstractValidator<DeleteVariantAttributeValueCommand>
    {
        public DeleteVariantAttributeValueCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Variant Attribute Value ID must be greater than 0");
        }
    }
}
