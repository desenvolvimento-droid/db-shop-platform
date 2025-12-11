using Shop.Common.Errors;
using Shop.Common.Resources;

namespace Shop.Application.Exceptions;

public class ProductError(string code, params object[] args)
    : RuleError(ProductMessages.ResourceManager, code, args);
