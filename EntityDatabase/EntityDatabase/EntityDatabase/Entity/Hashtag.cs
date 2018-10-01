using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Hashtag
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
        
        public List<HashtagInPost> HashtagInPosts { get; set; }
    }
}
