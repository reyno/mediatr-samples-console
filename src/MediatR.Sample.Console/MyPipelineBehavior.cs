using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRSampleConsole {

    public class MyPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>  {
        private readonly IServiceProvider _serviceProvider;

        public MyPipelineBehavior(
            IServiceProvider serviceProvider
            ) {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {

            // validate this request
            await HandleValidation(request, cancellationToken);

            // call the next behavior in the pipeline
            return await next();

        }

        private async Task HandleValidation(TRequest request, CancellationToken cancellationToken) {

            // find all validators for this request
            var validators = _serviceProvider.GetServices<RequestValidator<TRequest>>();

            // if no validators are found for this request, just exit
            if (!validators.Any()) return;

            // run all validations
            var validationResults = await Task.WhenAll(validators.Select(x => x.ValidateAsync(request)));

            // get any failed results
            var failedValidationResults = validationResults.Where(result => !result.IsValid);

            // check for failures
            if (failedValidationResults.Any()) {
                // throw an exception with all the errors
                throw new FluentValidation.ValidationException(
                    failedValidationResults.SelectMany(x => x.Errors)
                    );

            }

        }
    }
}
