using FluentValidation;

namespace MediatRSampleConsole {

    public interface IRequestValidator { }
    public abstract class RequestValidator<TRequest> :  AbstractValidator<TRequest>, IRequestValidator { }

}
