using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Products.Command.DeleteProduct
{
    public class DeleteProductCommand(int id) : IRequest
    {
        public int Id { get; set; } = id;
    }
}
