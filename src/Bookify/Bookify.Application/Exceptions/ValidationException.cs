using Bookify.Application.Abstractions.Behaviors;

namespace Bookify.Application.Exceptions
{
    public class ValidationException(IEnumerable<ValidationError> errors) : Exception
    {
        public IEnumerable<ValidationError> Errors { get; } = errors;
    }
}
