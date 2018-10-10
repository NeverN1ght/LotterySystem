namespace LotterySystem.MessageBus.Messages
{
    public enum MessageTypes : byte
    {
        AddNewUserCommand,
        AddNewAlbumCommand,
        AddNewPhotoCommand,
        GetPhotoCountCommand,
        PhotoCountMessage
    }
}
