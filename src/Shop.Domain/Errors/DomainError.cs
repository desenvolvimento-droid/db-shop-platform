using FluentResults;

namespace Shop.Domain.Exceptions
{
    public class DomainError : Error
    {
        public DomainError() { }

        public DomainError(string message) : base(message) { }
    }
}
