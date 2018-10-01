using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }

        public int? User1Id { get; set; }
        public User User1 { get; set; }
        
        public int? User2Id { get; set; }
        public User User2 { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
