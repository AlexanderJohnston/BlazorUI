using Serilog;

namespace BlazorUI.Server.PostSharp
{
    public class LogConfiguration
    {
        public LoggerConfiguration VerboseLogger(LoggerConfiguration config)
        {
            config.MinimumLevel.Verbose()
                  .Enrich.FromLogContext()
                  .Enrich.WithThreadId()
                  .WriteTo.Console(outputTemplate: Template, theme: ConsoleExtensions.BlueConsole);
            return config;
        }

        public const string Template =
                "{Timestamp:yyyy-MM-dd HH:mm:ss} |{Level:u3}: [{ThreadId}:{SourceContext}]{Indent:l} {Message:lj}{NewLine}{Exception}";
    }
}