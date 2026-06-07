using ECommerce.Application.ProductVariants.Commands.CreateProductVariant;
using ECommerce.Application.ProductVariants.Commands.UpdateProductVariant;
using ECommerce.Application.ProductVariants.Commands.DeleteProductVariant;
using ECommerce.Application.ProductVariants.Queries.GetProductVariantById;
using ECommerce.Application.ProductVariants.Queries.GetProductVariantsForProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VariantsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateVariant(CreateProductVariantCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetVariantById), new { id }, id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVariantById(int id)
        {
            var query = new GetProductVariantByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetVariantsForProduct(int productId)
        {
            var query = new GetProductVariantsForProductQuery(productId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVariant(int id, UpdateProductVariantCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVariant(int id)
        {
            var command = new DeleteProductVariantCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
