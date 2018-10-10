using System.Threading.Tasks;
using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.Data.Abstractions
{
    public interface IUserService
    {
        Task AddNewUser(UserDto userDto);
    }
}
