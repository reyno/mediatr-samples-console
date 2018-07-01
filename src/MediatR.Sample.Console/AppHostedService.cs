using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MediatRSampleConsole.Requests;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediatRSampleConsole {
    public class AppHostedService : IHostedService, IDisposable {
        private readonly ILogger<AppHostedService> _logger;
        private readonly IMediator _mediator;

        public AppHostedService(
            ILogger<AppHostedService> logger,
            IMediator mediator
            ) {
            _logger = logger;
            _mediator = mediator;
        }

        public void Dispose() {
            //
        }

        public async Task StartAsync(CancellationToken cancellationToken) {

            await BasicRequest();
            await WithValidationRequest();


        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;


        private async Task BasicRequest() {

            _logger.LogInformation("Running basic request...");

            var message = await _mediator.Send(new BasicRequest {
                Name = "Bob"
            });

            _logger.LogInformation($" - {message}");

        }
        private async Task WithValidationRequest() {

            _logger.LogInformation("Running request with validation error");

            try {

                var message = await _mediator.Send(new WithValidationRequest { });

            } catch (ValidationException validationException) {

                _logger.LogError("Request failed");
                foreach(var error in validationException.Errors)
                    _logger.LogWarning($" - {error.ErrorMessage}");

            }
        }

    }
}
