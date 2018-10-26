using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class UserDto : BaseDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Gender Gender { get; set; }
        public bool IsAdmin { get; set; }
        
        public DateTime Birthdate { get; set; }
    }
}
