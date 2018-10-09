using System.Collections.Generic;
using LotterySystem.Data.Abstractions;

namespace LotterySystem.Data.Entities
{
    public class User : Entity
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}
