using ECommerce.Application.AttributeValues.Commands.CreateAttributeValue;
using ECommerce.Application.AttributeValues.Queries.GetAttributeValueById;
using ECommerce.Application.AttributeValues.Queries.GetAttributeValuesForAttribute;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttributeValuesController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAttributeValue(CreateAttributeValueCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAttributeValueById), new { id }, id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttributeValueById(int id)
        {
            var query = new GetAttributeValueByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("attribute/{productAttributeId}")]
        public async Task<IActionResult> GetAttributeValuesForAttribute(int productAttributeId)
        {
            var query = new GetAttributeValuesForAttributeQuery(productAttributeId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
