namespace AlfieBot.Config
{
    public class EnvironmentSettings
    {
        public string LocalDev { get; set; }

        public bool IsLocalDev => !string.IsNullOrWhiteSpace(this.LocalDev);
    }
}
