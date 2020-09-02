namespace AlfieBot.Commands
{
    using DSharpPlus.CommandsNext;

    public static class CommandsNextModuleExtensions
    {
        public static void RegisterTestCommands(this CommandsNextExtension module)
        {
            module.RegisterCommands<Haldo>();
            module.RegisterCommands<UserLevels>();
        }
        /// <summary>
        /// Registers basic commands.
        /// </summary>
        /// <remarks>Add new commands here if they're defined in this project.</remarks>
        public static void RegisterBasicCommands(this CommandsNextExtension module)
        {
            module.RegisterCommands<DiceRoller>();
        }
    }
}
