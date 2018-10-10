using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.MessageBus.Messages
{
    public class AddNewPhotoCommand : BaseMessage
    {
        public PhotoDto PhotoDto { get; set; }
    }
}
