namespace AlfieBot.Abstractions.Models
{
    using System;

    public class QueueNameAttribute : Attribute
    {
        public string QueueName { get; }

        public QueueNameAttribute(string name)
        {
            this.QueueName = name;
        }
    }
}
