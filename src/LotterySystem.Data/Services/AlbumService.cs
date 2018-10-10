using System.Threading.Tasks;
using LotterySystem.Data.Abstractions;
using LotterySystem.Data.Entities;
using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.Data.Services
{
    public class AlbumService: IAlbumService
    {
        private readonly IRepository _repository;

        public AlbumService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task AddNewAlbum(AlbumDto albumDto)
        {
            await _repository.AddAsync<Album>(new Album {
                UserId = albumDto.UserId,
                Title = albumDto.Title
            });
        }
    }
}
