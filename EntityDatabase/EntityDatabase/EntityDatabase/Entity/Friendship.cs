using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }

        public int? ApplicantId { get; set; }
        public User Applicant { get; set; }
        
        public int? RecipientId { get; set; }
        public User Recipient { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
