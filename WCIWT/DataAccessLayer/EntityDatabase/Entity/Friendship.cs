using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure;

namespace EntityDatabase
{
    public class Friendship : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid? ApplicantId { get; set; }
        public virtual User Applicant { get; set; }
        
        public Guid? RecipientId { get; set; }
        public virtual User Recipient { get; set; }

        public bool IsConfirmed { get; set; }

        [NotMapped]
        public string TableName => nameof(WCIWTDbContext.Friendships);
    }
}
