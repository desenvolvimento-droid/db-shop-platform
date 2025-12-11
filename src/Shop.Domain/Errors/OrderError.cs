using Shop.Common.Errors;
using Shop.Common.Resources;

namespace Shop.Application.Errors;

public class OrderError(string code, params object[] args)
    : RuleError(OrderMessages.ResourceManager, code, args);
