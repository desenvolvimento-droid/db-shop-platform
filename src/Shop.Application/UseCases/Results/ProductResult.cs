namespace Shop.Application.UseCases.Results;

public class ProductResult
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public decimal Price { get; set; }
}
