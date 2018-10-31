using BusinessLayer.DataTransferObjects.Common;

namespace BusinessLayer.QueryObjects
{
    public class UserFilterDto : FilterDtoBase
    {
        public string Username { get; set; }
    }
}