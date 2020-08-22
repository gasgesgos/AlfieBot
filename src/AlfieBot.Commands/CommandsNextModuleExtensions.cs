namespace AlfieBot.Commands
{
    using DSharpPlus.CommandsNext;

    public static class CommandsNextModuleExtensions
    {
        /// <summary>
        /// Registers basic commands.
        /// </summary>
        /// <remarks>Add new commands here if they're defined in this project.</remarks>
        public static void RegisterBasicCommands(this CommandsNextExtension module)
        {
            module.RegisterCommands<Haldo>();
            module.RegisterCommands<DiceRoller>();
            module.RegisterCommands<UserLevels>();
        }
    }
}
