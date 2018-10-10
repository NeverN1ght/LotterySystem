using LotterySystem.Data.Abstractions;

namespace LotterySystem.Data.Entities
{
    public class Photo : Entity
    {
        public int AlbumId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int Likes { get; set; }

        public Album Album { get; set; }
    }
}
