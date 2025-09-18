using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

namespace Templates.Api.Infrastructure.Extensions
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddSerilogLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .MinimumLevel.Override(Assembly.GetEntryAssembly()?.GetName().Name, LogEventLevel.Error)
                .WriteTo.PostgreSQL(
                    connectionString: configuration.GetConnectionString("DefaultConnection"),
                    tableName: "EventLog",
                    needAutoCreateTable: true,
                    restrictedToMinimumLevel: LogEventLevel.Error)
                .WriteTo.Debug()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            return services;
        }
    }
}
