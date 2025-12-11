using FluentValidation;

namespace Shop.Application.UseCases.Orders.ChangeOrderStatus
{
    public class ChangeOrderStatusValidator : AbstractValidator<ChangeOrderStatusCommand>
    {
        public ChangeOrderStatusValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.OrderStatus)
                .IsInEnum();
        }
    }
}
