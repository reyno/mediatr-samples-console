using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRSampleConsole.Requests {

    public class BasicRequest : IRequest<string> {
        public string Name { get; set; }
    }

    public class BasicRequestHandler : IRequestHandler<BasicRequest, string> {
        public Task<string> Handle(BasicRequest request, CancellationToken cancellationToken) {
            return Task.FromResult($"Hello {request.Name}");
        }
    }

}
