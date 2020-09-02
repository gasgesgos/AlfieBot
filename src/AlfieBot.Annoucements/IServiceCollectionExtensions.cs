namespace AlfieBot.Annoucements
{
    using DSharpPlus.CommandsNext;

    public static class Extensions
    {
        /// <summary>
        /// Registers announcement commands.
        /// </summary>
        public static void RegisterAnnouncementCommands(this CommandsNextExtension module)
        {
            module.RegisterCommands<AnnouncementCommands>();
        }
    }
}
