using BusinessLayer.DataTransferObjects;
using PresentationLayerMVC.Models.Posts;

namespace PresentationLayerMVC.Models.Aggregated
{
    public class UserProfileAggregatedViewModel
    {
        public PostListViewModel PostListViewModel { get; set; }
        public UserDto User { get; set; }
    }
}