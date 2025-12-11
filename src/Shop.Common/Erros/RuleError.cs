using FluentResults;
using System.Resources;

namespace Shop.Common.Errors;

public class RuleError : Error
{
    public string Code { get; private set; }

    public RuleError(ResourceManager resourceManager, string code, params object[] args)
        : base(FormatMessage(resourceManager, code, args))
    {
        Code = code;
    }

    private static string FormatMessage(ResourceManager resourceManager, string code, params object[] args)
    {
        string? template = resourceManager.GetString(code);

        if (string.IsNullOrWhiteSpace(template))
            throw new AccessViolationException($"Unknown error code '{code}'");

        return string.Format(template, args);
    }

}
