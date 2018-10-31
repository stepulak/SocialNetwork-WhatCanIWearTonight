using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using System;

namespace BusinessLayer.QueryObjects
{
    public class UserFilterDto : FilterDtoBase
    {
        public string Username { get; set; }
        public Gender Gender { get; set; }
        public DateTime BornBefore { get; set; }
        public DateTime BornAfter { get; set; }

    }
}