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
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ImageId { get; set; }
        public virtual Image Image { get; set; }

        public VoteType Type { get; set; }
    }
}
