namespace AlfieBot.Config
{
    public class ConfigurationSettings
    {
        public string LocalDev { get; set; }

        public bool IsLocalDev => !string.IsNullOrWhiteSpace(this.LocalDev);
    }
}
