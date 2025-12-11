using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Controllers.Base;
using Shop.Application.UseCases.Orders.ChangeOrderStatus;
using Shop.Application.UseCases.Orders.CreateOrder;
using Shop.Application.UseCases.Orders.GetOrderById;
using Shop.Application.UseCases.Orders.GetOrders;
using Shop.Application.UseCases.Results;
using Shop.Common.Constants;
using Shop.Domain.Aggregates.OrderAggregate;
using Shop.Models.Requests;

namespace Shop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController(ISender mediator, IMapper mapper, ILogger<OrdersController> logger) 
        : BaseController(logger)
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetOrders()
        {
            var ordersResults = await mediator.Send(new GetOrdersQuery());

            return Ok(ordersResults);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderResult>> GetOrderById(Guid orderId)
        {
            var orderResult = await mediator.Send(new GetOrderByIdQuery(orderId));
            if(orderResult == null)
                return NoContent();

            return Ok(orderResult);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest orderRequest)
        {
            var orderCmd = mapper.Map<CreateOrderCommand>(orderRequest);
            var cmdResult = await mediator.Send(orderCmd);
            if (cmdResult.IsFailed)
                return HandleResult(cmdResult);

            return CreatedAtAction(nameof(GetOrderById), ControllerConstants.Orders,
                cmdResult.Value);
        }

        [HttpPut("{orderId}/Status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeOrderStatus(Guid orderId, [FromBody] OrderStatus orderStatus)
        {
            var cmdResult = await mediator.Send(new ChangeOrderStatusCommand(orderId, orderStatus));

            return HandleResult(cmdResult);
        }
    }
}
