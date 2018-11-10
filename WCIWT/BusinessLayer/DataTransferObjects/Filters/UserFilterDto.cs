using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using System;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class UserFilterDto : FilterDtoBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime BornBefore { get; set; }
        public DateTime BornAfter { get; set; }

        public UserFilterDto()
        {
            Gender = Gender.NoInformation;
            BornBefore = DateTime.MinValue;
            BornAfter = DateTime.MinValue;
        }
    }
}