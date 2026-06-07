using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Products.Command.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal BasePrice { get; set; }
    }
}
