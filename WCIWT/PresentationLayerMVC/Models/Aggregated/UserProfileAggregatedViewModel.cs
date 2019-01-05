using BusinessLayer.DataTransferObjects;
using PresentationLayerMVC.Models.Posts;

namespace PresentationLayerMVC.Models.Aggregated
{
    public class UserProfileAggregatedViewModel
    {
        public bool Found { get; set; } = true;
        public PostListViewModel PostListViewModel { get; set; }
        public UserDto User { get; set; }
        public bool HasPendingFriendRequest { get; set; }
        public bool IsFriend { get; set; }
        public int Page { get; set; }
    }
}