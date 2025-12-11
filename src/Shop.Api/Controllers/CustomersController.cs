using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Controllers.Requests;
using Shop.Application.UseCases.Customers.CreateCustomer;
using Shop.Application.UseCases.Customers.GetCustomerById;
using Shop.Application.UseCases.Customers.GetCustomers;
using Shop.Application.UseCases.Customers.UpdateCustomer;
using Shop.Common.Constants;
using Shop.Models.Requests;

namespace Shop.Api.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CustomersController(
        ISender mediator) : Controller
    {
        private readonly ISender _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCustomers()
        {
            var customersDto = await _mediator.Send(new GetCustomersQuery());
            return Ok(customersDto);
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCustomerById(Guid customerId)
        {
            var customerDto = await _mediator.Send(new GetCustomerByIdQuery(customerId));
            if (customerDto == null)
            {
                return NotFound();
            }

            return Ok(customerDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer(CreateCustomerRequest customerRequest)
        {
            var customerCmd = new CreateCustomerCommand(customerRequest.Name);
            var cmdResult = await _mediator.Send(customerCmd);

            return CreatedAtAction(nameof(GetCustomerById), ControllerConstants.Customers,
                cmdResult.Value);
        }

        [HttpPut("{customerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, UpdateCustomerRequest customerRequest)
        {
            if (customerId != customerRequest.Id)
            {
                return BadRequest();
            }

            var customerCmd = new UpdateCustomerCommand(customerRequest.Id, customerRequest.Name);
            await _mediator.Send(customerCmd);

            return NoContent();
        }
    }
}
