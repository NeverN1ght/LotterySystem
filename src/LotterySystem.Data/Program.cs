using System;
using System.Threading.Tasks;
using LotterySystem.Data.Contexts;
using LotterySystem.Data.Repositories;
using LotterySystem.Data.Services;
using LotterySystem.MessageBus;
using LotterySystem.MessageBus.Messages;
using LotterySystem.MessageBus.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LotterySystem.Data
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static AlbumService _albumService;
        private static UserService _userService;
        private static PhotoService _photoService;

        private static KafkaProducer _producer;
        private static KafkaConsumer _consumer;

        static void Main(string[] args)
        {
            Console.WriteLine(KafkaConfiguration.GeneratorDataTopic);
            Console.WriteLine(KafkaConfiguration.ActivityDataTopic);

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            MainAsync(args).GetAwaiter().GetResult();

            Console.ReadKey();
        }

        private static async Task MainAsync(string[] args)
        {
            var context = new SqlDbContext(new DbContextOptionsBuilder<SqlDbContext>()
                .UseSqlServer(Configuration["DatabaseConnection"])
                .Options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
            _albumService = new AlbumService(new SqlRepository(context));
            _userService = new UserService(new SqlRepository(context));
            _photoService = new PhotoService(new SqlRepository(context));

            _consumer = new KafkaConsumer();
            _producer = new KafkaProducer();

            Task.Run(() => _consumer.SubscribeOn<string>(new[] {KafkaConfiguration.GeneratorDataTopic, KafkaConfiguration.ActivityDataTopic}, HandleMessage));
        }

        private static async Task HandleMessage(string message)
        {
            var msgObject = JObject.Parse(message);

            switch (msgObject["Type"].ToObject<MessageTypes>())
            {
                case MessageTypes.AddNewUserCommand:
                    Console.WriteLine($"{DateTime.UtcNow} | Received {nameof(MessageTypes.AddNewUserCommand)}");
                    await _userService.AddNewUser(
                        msgObject["Message"].ToObject<AddNewUserCommand>().UserDto);
                    Console.WriteLine($"{DateTime.UtcNow} | Successfully executed {nameof(MessageTypes.AddNewUserCommand)}");
                    break;
                case MessageTypes.AddNewAlbumCommand:
                    Console.WriteLine($"{DateTime.UtcNow} | Received {nameof(MessageTypes.AddNewAlbumCommand)}");
                    await _albumService.AddNewAlbum(
                        msgObject["Message"].ToObject<AddNewAlbumCommand>().AlbumDto);
                    Console.WriteLine($"{DateTime.UtcNow} | Successfully executed {nameof(MessageTypes.AddNewAlbumCommand)}");
                    break;
                case MessageTypes.AddNewPhotoCommand:
                    Console.WriteLine($"{DateTime.UtcNow} | Received {nameof(MessageTypes.AddNewPhotoCommand)}");
                    await _photoService.AddNewPhoto(
                        msgObject["Message"].ToObject<AddNewPhotoCommand>().PhotoDto);
                    Console.WriteLine($"{DateTime.UtcNow} | Successfully executed {nameof(MessageTypes.AddNewPhotoCommand)}");
                    break;
                case MessageTypes.GetPhotoCountCommand:
                    Console.WriteLine($"{DateTime.UtcNow} | Received {nameof(MessageTypes.GetPhotoCountCommand)}");
                    await _producer.SendAsync(
                        KafkaConfiguration.ActivityDataTopic,
                        JsonConvert.SerializeObject(
                            new MessageWrapper {
                                Message = new PhotoCountMessage {
                                    PhotoCount = await _photoService.GetPhotoCountAsync()
                                },
                                Type = MessageTypes.PhotoCountMessage
                            }));
                    Console.WriteLine($"{DateTime.UtcNow} | Sent new {nameof(MessageTypes.PhotoCountMessage)}");
                    break;
            }
        }
    }
}
