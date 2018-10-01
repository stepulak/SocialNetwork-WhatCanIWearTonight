using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Vote
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        public User User { get; set; }

        [Key, Column(Order = 1)]
        public int ImageId { get; set; }
        public Image Image { get; set; }

        public VoteType Type { get; set; }
    }
}
