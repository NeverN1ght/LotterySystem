using System.Threading.Tasks;
using LotterySystem.Data.Abstractions;
using LotterySystem.Data.Entities;
using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.Data.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IRepository _repository;

        public PhotoService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task AddNewPhoto(PhotoDto photoDto)
        {
            await _repository.AddAsync<Photo>(new Photo {
                Title = photoDto.Title,
                AlbumId = photoDto.AlbumId,
                Url = photoDto.Url,
                Likes = photoDto.Likes
            });
        }

        public async Task<int> GetPhotoCountAsync()
        {
            return await _repository.GetEntityCountAsync<Photo>();
        }
    }
}
