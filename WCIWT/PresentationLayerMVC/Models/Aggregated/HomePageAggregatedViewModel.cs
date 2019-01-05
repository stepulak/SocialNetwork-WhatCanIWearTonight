using PresentationLayerMVC.Models.FriendRequests;
using PresentationLayerMVC.Models.Friends;
using PresentationLayerMVC.Models.Posts;

namespace PresentationLayerMVC.Models.Aggregated
{
    public class HomePageAggregatedViewModel
    {
        public PostListViewModel PostListViewModel;
        public FriendRequestListViewModel FriendRequestListViewModel;
        public FriendListViewModel FriendListViewModel;
        public int Page;
    }
}