using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.ProductAttributes.Dtos
{
    public class ProductAttributeDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
    }
}
