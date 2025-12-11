using Shop.Common.Resources;

namespace Shop.Application.UseCases.Results;

public class CommandSucceeded
{
    public Guid Id { get; private set; }
    public string Message { get; private set; }

    public CommandSucceeded(Guid id, string message, DateTime dateTime)
    {
        Id = id;
        var template = OrderMessages.ResourceManager.GetString(message);

        if (string.IsNullOrWhiteSpace(template))
            throw new InvalidOperationException($"Unknown error code '{message}'");

        Message = string.Format(template, id);
    }
}