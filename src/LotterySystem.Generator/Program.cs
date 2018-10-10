using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LotterySystem.Generator.DataModels;
using LotterySystem.Generator.Services;
using LotterySystem.MessageBus;
using LotterySystem.MessageBus.Dtos;
using LotterySystem.MessageBus.Messages;
using LotterySystem.MessageBus.Wrappers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LotterySystem.Generator
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static List<int> _albumIds = new List<int>();

        static void Main(string[] args)
        {
            Console.WriteLine(KafkaConfiguration.GeneratorDataTopic);

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            await GetPretendentsAsync(
                    new Random(),
                    new ApiService(Configuration["ApiBaseUrl"]),
                    new KafkaProducer(),
                    KafkaConfiguration.GeneratorDataTopic)
                .ContinueWith(t =>
                    StartGeneratingAsync(
                        new Random(),
                        new ApiService(Configuration["ApiBaseUrl"]),
                        new KafkaProducer(),
                        KafkaConfiguration.GeneratorDataTopic,
                        (50, 100)));

            Console.ReadKey();
        }

        private static async Task StartGeneratingAsync(
            Random rnd, 
            ApiService api, 
            KafkaProducer producer, 
            string topic,
            (int from, int to) timeoutRange)
        {
            while (true)
            {
                await producer.SendAsync(
                    topic,
                    JsonConvert.SerializeObject(
                        new MessageWrapper {
                            Message = new AddNewPhotoCommand {
                                PhotoDto = await GetRandomPhotoAsync(rnd, api)
                            },
                            Type = MessageTypes.AddNewPhotoCommand
                        }));
                Console.WriteLine($"{DateTime.UtcNow} | Sent new {nameof(AddNewPhotoCommand)}");

                await Task.Delay(rnd.Next(timeoutRange.from, timeoutRange.to));
            }
        }

        private static async Task GetPretendentsAsync(
            Random rnd, 
            ApiService api, 
            KafkaProducer producer, 
            string topic)
        {
            var users = new List<UserModel>();
            for (var t = 1; t <= 10; t++)
            {
                users.Add(
                    JsonConvert.DeserializeObject<UserModel>(
                        await api.GetDataAsync($"/users/{t}")));
            }

            var albums = new List<AlbumModel>();
            for (var i = 0; i < 20; i++)
            {
                var id = rnd.Next(1, 101);
                if (!_albumIds.Contains(id))
                {
                    _albumIds.Add(id);
                    albums.Add(
                        JsonConvert.DeserializeObject<AlbumModel>(
                            await api.GetDataAsync($"/albums/{id}")));
                }
            }

            await SendUsersAddCommandsAsync(users, producer, topic);
            await SendAlbumsAddCommandsAsync(albums, producer, topic);
        }

        private static async Task<PhotoDto> GetRandomPhotoAsync(Random rnd, ApiService api)
        {
            var json = await api.GetDataAsync($"/photos/{rnd.Next(1, 5000)}");
            var photo = JsonConvert.DeserializeObject<PhotoDto>(json);
            if (photo.AlbumId > 20)
            {
                photo = await GetRandomPhotoAsync(rnd, api);
            }
                
            return photo;
        }

        private static async Task SendUsersAddCommandsAsync(List<UserModel> users, KafkaProducer producer, string topic)
        {
            foreach (var u in users)
            {
                await producer.SendAsync(
                    topic,
                    JsonConvert.SerializeObject(
                        new MessageWrapper {
                            Message = new AddNewUserCommand {
                                UserDto = new UserDto {
                                    Email = u.Email,
                                    FullName = u.Name,
                                    Username = u.Username
                                }
                            },
                            Type = MessageTypes.AddNewUserCommand
                        }));
                Console.WriteLine($"{DateTime.UtcNow} | Sent new {nameof(AddNewUserCommand)}");
            }
        }

        private static async Task SendAlbumsAddCommandsAsync(List<AlbumModel> albums, KafkaProducer producer, string topic)
        {
            foreach (var a in albums)
            {
                await producer.SendAsync(
                    topic,
                    JsonConvert.SerializeObject(
                        new MessageWrapper {
                            Message = new AddNewAlbumCommand {
                                AlbumDto = new AlbumDto {
                                    UserId = a.UserId,
                                    Title = a.Title
                                }
                            },
                            Type = MessageTypes.AddNewAlbumCommand
                        }));
                Console.WriteLine($"{DateTime.UtcNow} | Sent new {nameof(AddNewAlbumCommand)}");
            }
        }
    }
}

