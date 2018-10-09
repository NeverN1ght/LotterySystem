using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using LotterySystem.Generator.DataModels;
using LotterySystem.Generator.Services;
using LotterySystem.MessageBus;
using Newtonsoft.Json;

namespace LotterySystem.Generator
{
    class Program
    {
        private static List<int> _albumIds;

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var serv = new ApiService("https://jsonplaceholder.typicode.com");

            var result = await serv.GetDataAsync("/users/3");

            var user = JsonConvert.DeserializeObject<UserModel>(result);

            Console.ReadKey();
        }

        private static async Task StartGenerating()
        {

        }

        private static async Task GetPretendents(Random rnd, ApiService api, KafkaProducer producer)
        {
            var albums = new List<AlbumModel>();
            for (var i = 0; i < 20; i++)
            {
                var id = rnd.Next(0, 101);
                if (!_albumIds.Contains(id))
                {
                    _albumIds.Add(id);
                    albums.Add(
                        JsonConvert.DeserializeObject<AlbumModel>(
                            await api.GetDataAsync($"/albums/{id}")));
                }
            }

            var userIds = new List<int>();
            var users = new List<UserModel>();
            albums.ForEach(async (a) => {
                //await producer.SendAsync()
                if (!userIds.Contains(a.UserId))
                {
                    userIds.Add(a.UserId);
                    users.Add(
                        JsonConvert.DeserializeObject<UserModel>(
                            await api.GetDataAsync($"/users/{a.UserId}")));
                }
            });

            users.ForEach(async () => await producer.SendAsync());
        }
    }
}

