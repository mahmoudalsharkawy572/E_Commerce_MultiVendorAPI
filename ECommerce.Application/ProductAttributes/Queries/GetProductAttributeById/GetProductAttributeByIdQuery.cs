using ECommerce.Application.ProductAttributes.Dtos;
using ECommerce.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.ProductAttributes.Queries.GetProductAttributeById
{
    public class GetProductAttributeByIdQuery(int id) : IRequest<ProductAttributeDto>
    {
        public int Id { get; set; } = id;
    }
}
