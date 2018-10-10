using System.Collections.Generic;
using LotterySystem.Data.Abstractions;

namespace LotterySystem.Data.Entities
{
    public class Album : Entity
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public User User { get; set; }
    }
}
