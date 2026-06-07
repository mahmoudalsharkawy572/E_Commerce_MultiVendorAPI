using ECommerce.Application.VariantAttributeValues.Commands.CreateVariantAttributeValue;
using ECommerce.Application.VariantAttributeValues.Commands.DeleteVariantAttributeValue;
using ECommerce.Application.VariantAttributeValues.Queries.GetVariantAttributeValues;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VariantAttributeValuesController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateVariantAttributeValue(CreateVariantAttributeValueCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetVariantAttributeValues), new { productVariantId = command.ProductVariantId }, id);
        }

        [HttpGet("variant/{productVariantId}")]
        public async Task<IActionResult> GetVariantAttributeValues(int productVariantId)
        {
            var query = new GetVariantAttributeValuesQuery(productVariantId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVariantAttributeValue(int id)
        {
            var command = new DeleteVariantAttributeValueCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
