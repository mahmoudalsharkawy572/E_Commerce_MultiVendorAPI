using FluentValidation;

namespace ECommerce.Application.AttributeValues.Commands.CreateAttributeValue
{
    public class CreateAttributeValueCommandValidator : AbstractValidator<CreateAttributeValueCommand>
    {
        public CreateAttributeValueCommandValidator()
        {
            RuleFor(x => x.ProductAttributeId)
                .GreaterThan(0)
                .WithMessage("Product Attribute ID must be greater than 0");

            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage("Value is required")
                .MaximumLength(255)
                .WithMessage("Value cannot exceed 255 characters");
        }
    }
}
