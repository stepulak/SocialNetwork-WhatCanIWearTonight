using BusinessLayer.DataTransferObjects;
using X.PagedList;

namespace PresentationLayerMVC.Models.Friends
{
    public class FriendListViewModel
    {
        public IPagedList<UserDto> Friends { get; set; }
        public UserDto Filter { get; set; }
    }
}