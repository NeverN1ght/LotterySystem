using System.Threading.Tasks;
using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.Data.Abstractions
{
    public interface IAlbumService
    {
        Task AddNewAlbum(AlbumDto albumDto);
    }
}
