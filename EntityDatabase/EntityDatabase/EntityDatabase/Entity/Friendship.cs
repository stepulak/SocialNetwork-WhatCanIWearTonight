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
        public int Id { get; set; }

        public int? ApplicantId { get; set; }
        public virtual User Applicant { get; set; }
        
        public int? RecipientId { get; set; }
        public virtual User Recipient { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
