using ECommerce.Application.ProductAttributes.Commands.CreateProductAttribute;
using ECommerce.Application.ProductAttributes.Queries.GetProductAttributeById;
using ECommerce.Application.ProductAttributes.Queries.GetAttributesForProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductAttiributeController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("{productId}/attributes")]
        public async Task<IActionResult> CreateAttribute(int productId, CreateProductAttributeCommand command)
        {
            var id = await _mediator.Send(command with { ProductId = productId });

            return CreatedAtAction(nameof(GetProductAttributeById), new { productId }, id);
        }

        [HttpGet("attributes/{id}")]
        public async Task<IActionResult> GetProductAttributeById(int id)
        {
            var query = new GetProductAttributeByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetAttributesForProductById(int productId)
        {
            var query = new GetAttributesForProductByIdQuery(productId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
