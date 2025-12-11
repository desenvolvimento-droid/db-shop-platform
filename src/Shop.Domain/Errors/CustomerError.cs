using Shop.Common.Errors;
using Shop.Common.Resources;

namespace Shop.Application.Exceptions;

public class CustomerError(string code, params object[] args)
    : RuleError(OrderMessages.ResourceManager, code, args);
