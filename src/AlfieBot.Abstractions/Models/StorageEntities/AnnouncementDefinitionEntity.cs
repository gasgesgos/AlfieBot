namespace AlfieBot.Abstractions.Models
{
    using System;

    public class AnnouncementDefinitionEntity : ChannelEntity
    {
        /// <summary>
        /// A key for the announcement entity.
        /// </summary>
        public override string RowKey => this.Name;

        /// <summary>
        /// A name for the announcement.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Text of the announcement.
        /// </summary>
        public string AnnouncementText { get; set; } = string.Empty;

        /// <summary>
        /// The number of seconds to wait before repeating the message.
        /// </summary>
        public int? RepeatSeconds { get; set; }

        /// <summary>
        /// The number of messages required before the previous announcement to allow another announcement.
        /// </summary>
        public int? AntiSpamBuffer { get; set; }
    }
}
