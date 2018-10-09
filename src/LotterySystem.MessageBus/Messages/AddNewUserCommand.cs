using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.MessageBus.Messages
{
    public class AddNewUserCommand : BaseMessage
    {
        public UserDto UserDto { get; set; }
    }
}
