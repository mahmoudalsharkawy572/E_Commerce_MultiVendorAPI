using ECommerce.Application.Common;
using ECommerce.Application.Products.Command.CreateProduct;
using ECommerce.Application.Products.Command.DeleteProduct;
using ECommerce.Application.Products.Command.UpdateProduct;
using ECommerce.Application.Products.Dtos;
using ECommerce.Application.Products.Queries.GetAllProducts;
using ECommerce.Application.Products.Queries.GetProductById;
using ECommerce.Domain.Common;
using ECommerce.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductDto>>>> GetAllProducts([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            var apiResponse = ApiResponse<PagedResult<ProductDto>>.Ok(result, "Products retrieved successfully!");
            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductById([FromRoute] int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            var apiResponse = ApiResponse<ProductDto>.Ok(product, "Product retrieved successfully!");
            return Ok(apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> CreateProduct([FromBody] CreateProductCommand command)
        {
            var productId = await _mediator.Send(command);
            var apiResponse = ApiResponse<int>.Ok(productId, "Product created successfully!");
            return Ok(apiResponse);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateRestaurant([FromRoute] int id, UpdateProductCommand updateProductCommand)
        {
            updateProductCommand.Id = id;
            await _mediator.Send(updateProductCommand);
            var apiResponse = ApiResponse<object>.Ok(updateProductCommand, $"Producct with id: {id} updated successfully!"); 
            return Ok(apiResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteRestaurantById([FromRoute] int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            var response = ApiResponse<object>.NoContent("Product deleted successfully!");
            return Ok(response);
        }

    }
}
