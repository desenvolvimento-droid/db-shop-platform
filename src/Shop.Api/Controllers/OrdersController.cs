using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.UseCases.Orders.ChangeOrderStatus;
using Shop.Application.UseCases.Orders.CreateOrder;
using Shop.Application.UseCases.Orders.GetOrderById;
using Shop.Application.UseCases.Orders.GetOrders;
using Shop.Common.Constants;
using Shop.Domain.Aggregates.OrderAggregate;
using Shop.Models.Requests;

namespace Shop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController(
        ISender mediator,
        IMapper mapper) : Controller
    {
        private readonly ISender _mediator = mediator;
        private readonly IMapper _mapper = mapper;


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetOrders()
        {
            var ordersDto = await _mediator.Send(new GetOrdersQuery());
            return Ok(ordersDto);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetOrderById(Guid orderId)
        {
            var orderDto = await _mediator.Send(new GetOrderByIdQuery(orderId));
            if (orderDto == null)
            {
                return NotFound();
            }

            return Ok(orderDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest orderRequest)
        {
            var orderCmd = _mapper.Map<CreateOrderCommand>(orderRequest);
            var cmdResult = await _mediator.Send(orderCmd);
            if (cmdResult.IsFailed)
            {

            }
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
            await _mediator.Send(new ChangeOrderStatusCommand(orderId, orderStatus));

            return NoContent();
        }
    }
}
