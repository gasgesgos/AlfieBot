namespace AlfieBot.Config
{
    using Microsoft.Extensions.Configuration;

    public class ConfigurationProvider
    {
        public static ConfigurationProvider Instance { get; private set; }

        public BotSettings BotSettings { get; private set; }

        public ConfigurationSettings ConfigSettings { get; private set; }

        static ConfigurationProvider()
        {
            Instance = new ConfigurationProvider();
        }

        private ConfigurationProvider()
        {
            var builder = new ConfigurationBuilder();

            // Load initial settings that are used to configure the loading of configuration
            builder.AddEnvironmentVariables("ConfigSettings");

            // Dev environment local secrets
            builder.AddUserSecrets<ConfigurationProvider>();

            var bootstrapRoot = builder.Build();
            this.ConfigSettings = bootstrapRoot.GetSection(nameof(ConfigurationSettings)).Get<ConfigurationSettings>() ?? new ConfigurationSettings();

            // Now load real settings
            builder = new ConfigurationBuilder();

            // Dev-environment user secrets should be associated with this configuration assembly (this overrides later providers)
            if (this.ConfigSettings.IsLocalDev)
            {
                builder.AddUserSecrets<ConfigurationProvider>();
            }

            // Add environment vars
            builder.AddEnvironmentVariables();
            

            var configRoot = builder.Build();

            this.BotSettings =  configRoot.GetSection(nameof(BotSettings)).Get<BotSettings>() ?? new BotSettings();
        }
    }
}
