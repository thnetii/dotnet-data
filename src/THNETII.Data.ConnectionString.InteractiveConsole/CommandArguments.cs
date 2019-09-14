using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using THNETII.Common;

namespace THNETII.Data.ConnectionString.InteractiveConsole
{
    public class CommandArguments
    {
        private static readonly string currentCultureKey = ConfigurationPath.Combine(
            nameof(CultureInfo), nameof(CultureInfo.CurrentCulture));
        private static readonly string currentUiCultureKey = ConfigurationPath.Combine(
            nameof(CultureInfo), nameof(CultureInfo.CurrentUICulture));

        public CommandArguments(CommandDefinition definition,
            ParseResult parseResult,
            IConfiguration configuration = null,
            ILogger<CommandArguments> logger = null)
        {
            if (definition is null)
                throw new ArgumentNullException(nameof(definition));
            if (parseResult is null)
                throw new ArgumentNullException(nameof(parseResult));
            if (configuration is null)
                configuration = new ConfigurationBuilder().Build();
            if (logger is null)
                logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<CommandArguments>.Instance;

            if (configuration.GetValue<string>(currentCultureKey).TryNotNull(out string currentCultureName))
            {
                CurrentCulture = GetCultureInfoByName(currentCultureName, logger);
            }
            if (configuration.GetValue<string>(currentUiCultureKey).TryNotNull(out string currentUiCultureName))
            {
                CurrentUICulture = GetCultureInfoByName(currentUiCultureName, logger);
            }
        }

        public CultureInfo CurrentCulture { get; }
        public CultureInfo CurrentUICulture { get; }

        public void ApplyCulture()
        {
            var (ci, uici) = (CurrentCulture, CurrentUICulture);
            if (!(ci is null))
                CultureInfo.CurrentCulture = ci;
            if (!(uici is null))
                CultureInfo.CurrentUICulture = uici;
        }

        [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = nameof(ILogger<CommandArguments>))]
        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        private static CultureInfo GetCultureInfoByName(string name, ILogger<CommandArguments> logger)
        {
            if (name is null)
                return null;
            if (string.IsNullOrWhiteSpace(name) ||
                string.Equals("inv", name, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("invariant", name, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(nameof(CultureInfo.InvariantCulture), name, StringComparison.OrdinalIgnoreCase))
            {
                return CultureInfo.InvariantCulture;
            }
            try
            {
                return CultureInfo.GetCultureInfo(name);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to get culture info with name {{{nameof(CultureInfo)}{nameof(CultureInfo.Name)}}}", name);
                return null;
            }
        }
    }
}
