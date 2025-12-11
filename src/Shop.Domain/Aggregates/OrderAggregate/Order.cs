using FluentResults;
using Shop.Domain.Core;
using Shop.Domain.Events;
using Shop.Domain.Exceptions;

namespace Shop.Domain.Aggregates.OrderAggregate
{
    public class Order : AggregateRoot
    {
        public Guid CustomerId { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public Address Address { get; private set; }
        public DateTime CreationDate { get; private set; }
        public List<OrderItem> OrderItems { get; private set; }


        public Order() { }

        public Order(Guid customerId, Address address)
        {
            ApplyChange(new OrderCreatedEvent(Guid.NewGuid(), customerId, OrderStatus.New, address, DateTime.UtcNow));
        }


        public void AddOrderItem(Guid productId, int quantity, decimal price)
        {
            ApplyChange(new OrderItemAddedEvent(Id, productId, quantity, price));
        }

        public Result SetNewStatus()
        {
            return new DomainError($"Is not possible to change the order status from {OrderStatus} to {OrderStatus.New}.");
        }

        public Result SetPaidStatus()
        {
            if (OrderStatus != OrderStatus.New)
            {
                return new DomainError($"Is not possible to change the order status from {OrderStatus} to {OrderStatus.Paid}.");
            }

            ApplyChange(new OrderStatusChangedEvent(Id, OrderStatus.Paid));

            return Result.Ok();
        }

        public Result SetShippedStatus()
        {
            if (OrderStatus != OrderStatus.Paid)
            {
                return new DomainError($"Is not possible to change the order status from {OrderStatus} to {OrderStatus.Shipped}.");
            }

            ApplyChange(new OrderStatusChangedEvent(Id, OrderStatus.Shipped));

            return Result.Ok();
        }

        public Result SetCancelledStatus()
        {
            if (OrderStatus == OrderStatus.Paid || OrderStatus == OrderStatus.Shipped)
            {
                return new DomainError($"Is not possible to change the order status from {OrderStatus} to {OrderStatus.Cancelled}.");
            }

            ApplyChange(new OrderStatusChangedEvent(Id, OrderStatus.Cancelled));

            return Result.Ok();
        }


        protected override void When(IDomainEvent @event)
        {
            switch (@event)
            {
                case OrderCreatedEvent e:
                    Handle(e);
                    break;
                case OrderItemAddedEvent e:
                    Handle(e);
                    break;
                case OrderStatusChangedEvent e:
                    Handle(e);
                    break;
            }
        }

        private void Handle(OrderCreatedEvent @event)
        {
            Id = @event.Id;
            CustomerId = @event.CustomerId;
            OrderStatus = @event.OrderStatus;
            Address = @event.Address;
            CreationDate = @event.CreationDate;
            OrderItems = new List<OrderItem>();
        }

        private void Handle(OrderItemAddedEvent @event)
        {
            var orderItem = OrderItems.SingleOrDefault(x => x.ProductId == @event.ProductId);
            if (orderItem != null)
            {
                orderItem.AddQuantity(@event.Quantity);
            }
            else
            {
                OrderItems.Add(new OrderItem(@event.ProductId, @event.Quantity, @event.Price));
            }
        }

        private void Handle(OrderStatusChangedEvent @event)
        {
            OrderStatus = @event.OrderStatus;
        }
    }
}
