using Shop.Domain.Aggregates.OrderAggregate;

namespace Shop.Application.UseCases.Results;

public class OrderResult
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }        

    public string CustomerName { get; set; } = default!;

    public string City { get; set; } = default!;

    public string Street { get; set; } = default!;

    public OrderStatus OrderStatus { get; set; }

    public DateTime CreationDate { get; set; }

    public List<OrderItemResult> OrderItems { get; set; } = new();

    public decimal TotalPrice { get; set; }

    public int TotalQuantity { get; set; }
}

public class OrderItemResult
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = default!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
