using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Products.Command.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Product name is required.")
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Description is required.")
                .MaximumLength(2000);

            RuleFor(x => x.BasePrice)
                .GreaterThan(0)
                .WithMessage("Base price must be greater than 0.");
        }
    }
}
