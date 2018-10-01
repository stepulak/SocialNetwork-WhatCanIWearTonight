using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class HashtagInPost
    {
        [Key, Column(Order = 0)]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [Key, Column(Order = 1)]
        public int HashtagId { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}
