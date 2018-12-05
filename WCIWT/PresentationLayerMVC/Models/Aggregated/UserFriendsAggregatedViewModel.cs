using BusinessLayer.DataTransferObjects;

namespace PresentationLayerMVC.Controllers
{
    public class UserFriendsAggregatedViewModel
    {
        public UserDto User { get; set; }
        public UserFriendsListViewModel UserFriendsList { get; set; }
    }
}