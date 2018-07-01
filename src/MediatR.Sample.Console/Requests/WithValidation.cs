using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

namespace MediatRSampleConsole.Requests {

    public class WithValidationRequest : IRequest<string> {
        public string Name { get; set; }
    }

    public class WithValidationRequestValidator : RequestValidator<WithValidationRequest> {
        public WithValidationRequestValidator() {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class WithValidationRequestHandler : IRequestHandler<WithValidationRequest, string> {
        private readonly IMediator _mediator;

        public WithValidationRequestHandler(
            IMediator mediator
            ) {
            _mediator = mediator;
        }

        public async Task<string> Handle(WithValidationRequest request, CancellationToken cancellationToken) {

            // just send this to the basic request so we don't duplicate logic
            return await _mediator.Send(new BasicRequest { Name = request.Name }, cancellationToken);

        }

    }

}
