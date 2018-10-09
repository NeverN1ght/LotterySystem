using System.Net;
using System.Threading.Tasks;

namespace LotterySystem.Generator.Services
{
    public class ApiService
    {
        private readonly string _baseUrl;

        public ApiService(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<string> GetDataAsync(string endpoint)
        {
            using (var webClient = new WebClient())
            {
                return await webClient.DownloadStringTaskAsync($"{_baseUrl}{endpoint}");
            }
        }
    }
}
