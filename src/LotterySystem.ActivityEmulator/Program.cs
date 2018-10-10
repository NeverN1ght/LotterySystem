using System;
using System.Threading.Tasks;
using LotterySystem.MessageBus;
using LotterySystem.MessageBus.Messages;
using LotterySystem.MessageBus.Wrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LotterySystem.ActivityEmulator
{
    class Program
    {
        private static KafkaProducer _producer;
        private static KafkaConsumer _consumer;

        static void Main(string[] args)
        {
            Console.WriteLine(KafkaConfiguration.ActivityDataTopic);
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadKey();
        }

        static async Task MainAsync(string[] args)
        {
            _producer = new KafkaProducer();
            _consumer = new KafkaConsumer();

            Task.Run(() => _consumer.SubscribeOn<string>(new[] {KafkaConfiguration.ActivityDataTopic}, HandleMessage));

            while (true)
            {
                await _producer.SendAsync(
                    KafkaConfiguration.ActivityDataTopic,
                    JsonConvert.SerializeObject(
                        new MessageWrapper {
                            Message = new GetPhotoCountCommand(),
                            Type = MessageTypes.GetPhotoCountCommand
                        }));
                Console.WriteLine($"{DateTime.UtcNow} | Sent new {nameof(MessageTypes.GetPhotoCountCommand)}");
                await Task.Delay(5000);
            }
        }

        private static async Task HandleMessage(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow} | Received {nameof(MessageTypes.PhotoCountMessage)}");
            var msgObject = JObject.Parse(message);
            var result = msgObject["Message"].ToObject<PhotoCountMessage>().PhotoCount;



            Console.WriteLine($"Messages: {result}");
        }
    }
}
