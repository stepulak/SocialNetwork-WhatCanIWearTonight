using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Friendship
    {
        public uint User1Id { get; set; }
        public User User1 { get; set; }

        public uint User2Id { get; set; }
        public User User2 { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
