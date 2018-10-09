using LotterySystem.MessageBus.Messages;

namespace LotterySystem.MessageBus.Wrappers
{
    public class MessageWrapper
    {
        public MessageTypes Type { get; set; }

        public BaseMessage Message { get; set; }
    }
}
