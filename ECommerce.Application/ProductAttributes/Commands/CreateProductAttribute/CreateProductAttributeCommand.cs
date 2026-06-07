using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.ProductAttributes.Commands.CreateProductAttribute
{
    public record CreateProductAttributeCommand(int ProductId,string Name) : IRequest<int>;
}
