using FluentValidation;

namespace MediatRSampleConsole {

    public abstract class RequestValidator<TRequest> :  AbstractValidator<TRequest> { }

}
