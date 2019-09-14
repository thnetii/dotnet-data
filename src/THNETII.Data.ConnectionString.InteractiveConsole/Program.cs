using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Data.ConnectionString.InteractiveConsole
{
    public static partial class Program
    {
        public static Task<int> Main(string[] args)
        {
            var cmdHandler = CommandHandler.Create<IHost, CancellationToken>(RunAsync);
            var cmdDef = new CommandDefinition(cmdHandler);
            var cmdParser = new CommandLineBuilder(cmdDef.RootCommand)
                .UseDefaults()
                .UseHost(Host.CreateDefaultBuilder, host =>
                {
                    host.ConfigureServices(services =>
                    {
                        services.AddSingleton(cmdDef);
                        services.AddSingleton<CommandArguments>();
                        services.AddScoped<CommandExecutor>();

                        services.AddSingleton<IPostConfigureOptions<InvocationLifetimeOptions>, InvocationLifetimeOptionsPostConfigure>();
                    });
                })
                .Build();

            return cmdParser.InvokeAsync(args);

            async Task RunAsync(IHost host, CancellationToken cancelToken)
            {
                var serviceProvider = host.Services;
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopeProvider = scope.ServiceProvider;
                    var executor = scopeProvider.GetRequiredService<CommandExecutor>();

                    await executor.ExecuteAsync(cancelToken).ConfigureAwait(false);
                }
            }
        }
    }
}
