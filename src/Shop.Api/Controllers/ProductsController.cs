using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Controllers.Base;
using Shop.Application.UseCases.Products.CreateProduct;
using Shop.Application.UseCases.Products.DeleteProduct;
using Shop.Application.UseCases.Products.GetProductById;
using Shop.Application.UseCases.Products.GetProducts;
using Shop.Application.UseCases.Products.UpdateProduct;
using Shop.Application.UseCases.Results;
using Shop.Common.Constants;
using Shop.Models.Requests;

namespace Shop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController(ISender mediator, ILogger<ProductsController> logger) 
        : BaseController(logger)
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductResult>>> GetProducts()
        {
            var productsDto = await mediator.Send(new GetProductsQuery());
            return Ok(productsDto);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductResult>> GetProductById(Guid productId)
        {
            var productDto = await mediator.Send(new GetProductByIdQuery(productId));
            if (productDto == null)
            {
                return NotFound();
            }

            return Ok(productDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct(CreateProductRequest productRequest)
        {
            var productCmd = new CreateProductCommand(productRequest.Name, productRequest.Price);
            var productCmdResult = await mediator.Send(productCmd);

            if (productCmdResult.IsFailed)
                return HandleResult(productCmdResult);

            return CreatedAtAction(
                nameof(GetProductById), 
                ControllerConstants.Products,
                productCmdResult.Value
            );
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(Guid productId, UpdateProductRequest productRequest)
        {
            if (productId != productRequest.Id)
            {
                return BadRequest();
            }

            var productCmd = new UpdateProductCommand(productRequest.Id, productRequest.Name, productRequest.Price);
            var productCmdResult = await mediator.Send(productCmd);

            return HandleResult(productCmdResult);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var productCmdResult = await mediator.Send(new DeleteProductCommand(productId));

            return HandleResult(productCmdResult);
        }
    }
}
