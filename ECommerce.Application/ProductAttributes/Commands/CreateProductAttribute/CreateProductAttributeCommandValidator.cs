using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.ProductAttributes.Commands.CreateProductAttribute
{
    public class CreateProductAttributeCommandValidator : AbstractValidator<CreateProductAttributeCommand>
    {
        public CreateProductAttributeCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
