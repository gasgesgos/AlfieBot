using System;

namespace AlfieBot.Abstractions.Models
{ 
    public class UserLevel : UserEntity
    {
        public override string RowKey => "UserLevel";

        public int? Level { get; set; }

        public static string GeneratePartitionKey(ulong serverId, ulong userId)
        {
            return UserEntity.FormatPartitionKey(serverId, userId);
        }
        public static string GenerateRowKey()
        {
            return "UserLevel";
        }
    }
}
