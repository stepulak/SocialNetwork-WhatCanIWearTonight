using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Vote
    {
        public uint UserId { get; set; }
        public User User { get; set; }

        public uint ImageId { get; set; }
        public Image Image { get; set; }
    }
}
