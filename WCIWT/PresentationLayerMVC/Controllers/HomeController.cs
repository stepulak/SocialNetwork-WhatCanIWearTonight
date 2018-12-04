using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Facades;
using PresentationLayerMVC.Models.Aggregated;
using PresentationLayerMVC.Models.FriendRequests;
using PresentationLayerMVC.Models.Friends;
using PresentationLayerMVC.Models.Posts;
using X.PagedList;

namespace PresentationLayerMVC.Controllers
{
    public class HomeController : Controller
    {
        public const int PostsPageSize = 10;
        public const int FriendRequestsPageSize = 20;
        public const int FriendsPageSize = 20;
        private const string FilterSessionKey = "filter";
        
        public UserFacade UserFacade { get; set; }
        public PostFacade PostFacade { get; set; }

        public PartialViewResult UserMenu()
        {
            var model = UserFacade.GetUserAsync(Guid.Parse("22d1461d-41db-4a5a-8996-dd0fcf7f5f04"));
            return PartialView("_UserMenu", model);
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            // TODO: If user is logged in, get his id
            var userId = Guid.Parse("22d1461d-41db-4a5a-8996-dd0fcf7f5f04");
           //var userId = Guid.Empty;
            var postsModel = await GetPostModel(userId, page);
            var friendRequestsModel = await GetFriendRequestsModel(userId);
            var friendsModel = await GetFriendsModel(userId);
            var homepageModel = new HomePageAggregatedViewModel
            {
                PostListViewModel = postsModel,
                FriendRequestListViewModel = friendRequestsModel,
                FriendListViewModel = friendsModel
            };
            return View("Index", homepageModel);
        }

        private async Task<FriendRequestListViewModel> GetFriendRequestsModel(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return new FriendRequestListViewModel
                {
                    FriendRequests = new StaticPagedList<FriendshipDto>(new List<FriendshipDto>(), 1, 0, 0)
                };
            }
            
            var friendshipRequests = await UserFacade.PendingFriendshipRequests(userId);
            var friendRequestsModel = InitializeFriendRequestListViewModel(friendshipRequests);

            return friendRequestsModel;
        }

        private async Task<FriendListViewModel> GetFriendsModel(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return new FriendListViewModel
                {
                    Friends = new StaticPagedList<UserDto>(new List<UserDto>(), 1, 0, 0)
                };
            }

            var friends = await UserFacade.GetFriendsOfUser(userId);
            return InitializeFriendListViewModel(friends);
        }
        

        private async Task<PostListViewModel> GetPostModel(Guid userId, int page)
        {
            // TODO: when filter DTO is changed, pass userId to filter
            var filter = Session[FilterSessionKey] as PostFilterDto ?? new PostFilterDto{PageSize = PostsPageSize};
            filter.RequestedPageNumber = page;
            var posts = await PostFacade.GetPostFeedAsync(filter, userId);
            return InitializePostListViewModel(posts);   
        }

        private FriendRequestListViewModel InitializeFriendRequestListViewModel(
            QueryResultDto<FriendshipDto, FriendshipFilterDto> result)
        {
            return new FriendRequestListViewModel
            {
                FriendRequests = new StaticPagedList<FriendshipDto>(result.Items, result.RequestedPageNumber ?? 1, FriendRequestsPageSize,
                    (int) result.TotalItemsCount),
                Filter = result.Filter
            };
        }

        private PostListViewModel InitializePostListViewModel(QueryResultDto<PostDto, PostFilterDto> result)
        {
            return new PostListViewModel
            {
                Posts = new StaticPagedList<PostDto>(result.Items, result.RequestedPageNumber ?? 1, PostsPageSize,
                    (int) result.TotalItemsCount),
                Filter = result.Filter
            };
        }

        private FriendListViewModel InitializeFriendListViewModel(IList<UserDto> result)
        {
            return new FriendListViewModel
            {
                Friends = new StaticPagedList<UserDto>(result, 1, FriendsPageSize, FriendsPageSize),
                Filter = null
            };
        }
    }
}