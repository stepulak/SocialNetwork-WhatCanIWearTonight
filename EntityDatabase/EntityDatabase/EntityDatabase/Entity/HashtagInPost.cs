using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class HashtagInPost
    {
        public uint PostId { get; set; }
        public Post Post { get; set; }
        
        public uint HashtagId { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}
