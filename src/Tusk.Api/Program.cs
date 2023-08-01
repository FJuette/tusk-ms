using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Tusk.Api.Extensions;

namespace Tusk.Api;
public static class Program
{
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
    public static int Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var isDevelopment = environment != "Staging" && environment != "Production";

        var levelSwitch = new LoggingLevelSwitch();

        var logConfig = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code, formatProvider: CultureInfo.InvariantCulture)
            .MinimumLevel.ControlledBy(levelSwitch)
            .MinimumLevel.Override("Microsoft", levelSwitch)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", levelSwitch);


        if (isDevelopment)
        {
            // Verbose logging on console
            logConfig
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
        }
        else
        {
            // Reduced logging in production mode, add more logging (Graylog, Apache Flink, ...) here
            logConfig
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
        }

        Log.Logger = logConfig.CreateLogger();

        try
        {
            Log.Information("Starting web host");
            CreateHostBuilder(args).Build().MigrateDatabase().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(
                webBuilder => webBuilder.UseStartup<Startup>());
}
