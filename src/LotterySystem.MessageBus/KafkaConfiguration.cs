using System;
using System.IO;

namespace LotterySystem.MessageBus
{
    public static class KafkaConfiguration
    {
        public static string GeneratorDataTopic { get; set; } = $"generator_data_{DateTime.UtcNow.Year}_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Hour}_{DateTime.UtcNow.Minute}_{DateTime.UtcNow.Second}";
        public static string ActivityDataTopic { get; set; } = $"activity_data_{DateTime.UtcNow.Year}_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Hour}_{DateTime.UtcNow.Minute}_{DateTime.UtcNow.Second}";
    }
}
