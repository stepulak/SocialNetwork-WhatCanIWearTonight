﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class PostReply
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Time { get; set; }

        public string Text { get; set; }
    }
}
