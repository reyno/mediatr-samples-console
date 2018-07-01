using System;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using MediatRSampleConsole.Requests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediatRSampleConsole {
    class Program {

        static async Task Main(string[] args) {

            var hostBuilder = BuildHost(args);

            // do any async initialization required here

            await hostBuilder.RunConsoleAsync();

        }

        public static IHostBuilder BuildHost(string[] args) => new HostBuilder()

            .ConfigureServices((context, services) => {

                // Add application services
                RegisterPipelineBehaviors(services);
                RegisterRequestValidators(services);

                // Add MediatR
                services.AddMediatR();

                // Add a service for doing the actual work
                services.AddSingleton<IHostedService, AppHostedService>();


            })

            .ConfigureLogging((context, logging) => {
                logging.AddConsole();
            })

            ;

        private static void RegisterPipelineBehaviors(IServiceCollection services) {

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MyPipelineBehavior<,>));

        }

        private static void RegisterRequestValidators(IServiceCollection services) {

            // find all the validator classes
            var requestValidatorTypes =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                where !assembly.IsDynamic
                from type in assembly.DefinedTypes

                where type.BaseType != null
                   && type.BaseType.IsGenericType
                   && type.BaseType.GetGenericTypeDefinition() == typeof(RequestValidator<>)

                select type
                ;

            // register all the validators in DI
            foreach (var type in requestValidatorTypes) {

                // find the request type from the validator
                var requestType = type.BaseType.GenericTypeArguments.FirstOrDefault();

                // add to DI
                services.AddSingleton(
                    typeof(RequestValidator<>).MakeGenericType(requestType),
                    type
                    );

            }


        }

    }
}
