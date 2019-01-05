using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class AdminDto : UserDto
    {
        public AdminDto()
        {
            IsAdmin = true;
            Gender = Gender.NoInformation;
        }
    }
}
