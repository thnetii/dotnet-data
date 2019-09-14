using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;

namespace THNETII.Data.ConnectionString.InteractiveConsole
{
    public class CommandDefinition
    {
        public CommandDefinition(ICommandHandler commandHandler)
        {
            var rootCommand = CreateRootCommand(commandHandler);

            RootCommand = rootCommand;
        }

        public RootCommand RootCommand { get; }

        private static string GetDescription() => typeof(Program).Assembly
            .GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        private static RootCommand CreateRootCommand(ICommandHandler commandHandler)
            => new RootCommand(GetDescription()) { Handler = commandHandler };
    }
}
