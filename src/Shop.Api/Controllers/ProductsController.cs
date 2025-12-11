using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.UseCases.Products.CreateProduct;
using Shop.Application.UseCases.Products.DeleteProduct;
using Shop.Application.UseCases.Products.GetProductById;
using Shop.Application.UseCases.Products.GetProducts;
using Shop.Application.UseCases.Products.UpdateProduct;
using Shop.Common.Constants;
using Shop.Models.Requests;

namespace Shop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController(
        ISender mediator) : Controller
    {
        private readonly ISender _mediator = mediator;


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetProducts()
        {
            var productsDto = await _mediator.Send(new GetProductsQuery());
            return Ok(productsDto);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetProductById(Guid productId)
        {
            var productDto = await _mediator.Send(new GetProductByIdQuery(productId));
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
            var productCmdResult = await _mediator.Send(productCmd);

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
            await _mediator.Send(productCmd);

            return NoContent();
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            await _mediator.Send(new DeleteProductCommand(productId));

            return NoContent();
        }
    }
}
