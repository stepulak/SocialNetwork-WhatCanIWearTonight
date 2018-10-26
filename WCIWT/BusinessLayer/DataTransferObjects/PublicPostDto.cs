using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class PublicPostDto : PostDto
    {
        public PublicPostDto()
        {
            HasAgeRestriction = false;
            GenderRestriction = Gender.NoInformation;
        }
    }
}
