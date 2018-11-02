using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class UserDto : DtoBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Gender Gender { get; set; }
        public bool IsAdmin { get; set; }
        
        public DateTime Birthdate { get; set; }
    }
}
