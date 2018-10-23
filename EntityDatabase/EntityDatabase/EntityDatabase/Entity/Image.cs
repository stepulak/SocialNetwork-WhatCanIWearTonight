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
    public class Image
    {
        [Key]
        public int Id { get; set; }
        
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public byte[] BinaryImage { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }

        public List<Vote> Votes { get; set; }
    }
}
