using System.Threading.Tasks;
using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.Data.Abstractions
{
    public interface IPhotoService
    {
        Task AddNewPhoto(PhotoDto photoDto);

        Task<int> GetPhotoCountAsync();
    }
}
