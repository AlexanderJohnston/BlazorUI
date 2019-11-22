using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorUI.Shared.Services
{
    public class PostsharpBackendLogging : IHostedService
    {
        /// <summary>
        ///   One day we need to actually implement support for the ILoggerFactory to share context with-
        ///   the generic host, but for now this is sufficient for Console logging whether semantic or not.
        /// </summary>
        readonly ILoggerFactory _factory;

        public PostsharpBackendLogging(ILoggerFactory factory)
        {
            _factory = factory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Generic implementation unrelated to Totem unless Serilog is doing magic.
            LoggingServices.DefaultBackend = new SerilogLoggingBackend(
                VerboseLogger.CreateLogger().ForContext<PostsharpBackendLogging>());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public LoggerConfiguration VerboseLogger => new LoggerConfiguration().MinimumLevel.Verbose()
                  .Enrich.FromLogContext()
                  .Enrich.WithThreadId()
                  .WriteTo.Console(outputTemplate: Template);

        public const string Template =
                "{Timestamp:yyyy-MM-dd HH:mm:ss} |{Level:u3}: [{ThreadId}:{SourceContext}]{Indent:l} {Message:lj}{NewLine}{Exception}";
    }
}
