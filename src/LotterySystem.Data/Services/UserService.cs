using System.Threading.Tasks;
using LotterySystem.Data.Abstractions;
using LotterySystem.Data.Entities;
using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task AddNewUser(UserDto userDto)
        {
            await _repository.AddAsync<User>(new User {
                Email = userDto.Email,
                Username = userDto.Username,
                FullName = userDto.FullName
            });
        }
    }
}
