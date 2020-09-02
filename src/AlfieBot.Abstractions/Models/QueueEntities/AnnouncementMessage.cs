namespace AlfieBot.Abstractions.Models
{
    using System;

    [QueueName("announcements")]
    public class AnnouncementMessage : ChannelBasedEntity, ICloneable
    {
        /// <summary>
        /// The name of the announcement.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An annoucement message.
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// The number of messages required to not trigger anti-spam measures.
        /// </summary>
        public int AntiSpamMessageCount { get; set; }

        /// <summary>
        /// The number of seconds before the next instance of this announcement.
        /// </summary>
        public int NextMessageIntervalSeconds { get; set; }

        /// <summary>
        /// The id value of the previous instance of this announcement.
        /// </summary>
        public ulong? PreviousAnnouncementMessageId{ get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
