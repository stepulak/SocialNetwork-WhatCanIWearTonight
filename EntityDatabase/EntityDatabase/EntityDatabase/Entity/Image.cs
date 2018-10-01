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
        public Post Post { get; set; }

        public byte[] BinaryImage { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public List<Vote> Votes { get; set; }
    }
}
