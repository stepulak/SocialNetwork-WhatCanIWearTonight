using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using PresentationLayerMVC.Models.Friends;
using X.PagedList;

namespace PresentationLayerMVC.Controllers
{
    public class UserFriendsListViewModel
    {
        public IPagedList<UserFriendViewModel> Friends { get; set; }
        public FriendshipFilterDto Filter { get; set; }
    }
}