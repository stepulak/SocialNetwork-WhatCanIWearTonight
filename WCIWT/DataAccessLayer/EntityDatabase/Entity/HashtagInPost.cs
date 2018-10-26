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
        [Key]
        public int Id { get; set; }
        
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        
        public int HashtagId { get; set; }
        public virtual Hashtag Hashtag { get; set; }
    }
}
