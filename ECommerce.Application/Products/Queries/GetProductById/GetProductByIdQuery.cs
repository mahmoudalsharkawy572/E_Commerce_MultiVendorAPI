using ECommerce.Application.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQuery(int id) : IRequest<ProductDto>
    {
        public int Id { get; set; } = id; 
    }
}
