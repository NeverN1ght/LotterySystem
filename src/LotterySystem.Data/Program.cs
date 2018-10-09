using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using LotterySystem.MessageBus;

namespace LotterySystem.Data
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
        }
    }
}
