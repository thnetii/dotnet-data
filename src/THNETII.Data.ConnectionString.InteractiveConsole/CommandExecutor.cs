using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using THNETII.Data.TextExtensions;
using System;

namespace THNETII.Data.ConnectionString.InteractiveConsole
{
    public class CommandExecutor
    {
        public CommandExecutor(
            CommandArguments arguments,
            IConfiguration configuration,
            ILogger<CommandExecutor> logger = null) : base()
        {
            Arguments = arguments;
            Configuration = configuration;
            Logger = logger ?? NullLogger<CommandExecutor>.Instance;
        }

        public CommandArguments Arguments { get; }
        public IConfiguration Configuration { get; }
        public ILogger<CommandExecutor> Logger { get; }

        public async Task ExecuteAsync(CancellationToken cancelToken = default)
        {
            Arguments.ApplyCulture();

            var tblFormatter = new DataTableTextFormatter
            {

            };
            var provTbl = DbProviderFactories.GetFactoryClasses();
            Console.WriteLine($"{typeof(DbProviderFactories).FullName}.{nameof(DbProviderFactories.GetFactoryClasses)}()");
            await tblFormatter.WriteToAsync(Console.Out, provTbl, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
