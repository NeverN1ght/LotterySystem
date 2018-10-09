using LotterySystem.MessageBus.Dtos;

namespace LotterySystem.MessageBus.Messages
{
    public class AddNewAlbumCommand : BaseMessage
    {
        public AlbumDto AlbumDto { get; set; }
    }
}
