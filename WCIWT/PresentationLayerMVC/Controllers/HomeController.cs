using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Facades;
using BusinessLayer.Facades.Common;
using PresentationLayerMVC.Models.Posts;
using X.PagedList;

namespace PresentationLayerMVC.Controllers
{
    public class HomeController : Controller
    {
        public const int PageSize = 5;

        private const string FilterSessionKey = "filter";
        
        public UserFacade UserFacage { get; set; }
        public PostFacade PostFacade { get; set; }
        
        public async Task<ActionResult> Index(int page = 1)
        {
            // TODO: If user is logged in, get his id
            var userId = Guid.Empty;
            
            var filter = Session[FilterSessionKey] as PostFilterDto ?? new PostFilterDto{PageSize = PageSize};
            filter.RequestedPageNumber = page;
            
            var result = await PostFacade.GetPostFeedAsync(filter);
            var model = InitializePostListViewModel(result);
            
            return View(model);
        }

        private PostListViewModel InitializePostListViewModel(QueryResultDto<PostDto, PostFilterDto> result)
        {
            return new PostListViewModel
            {
                Posts = new StaticPagedList<PostDto>(result.Items, result.RequestedPageNumber ?? 1, PageSize,
                    (int) result.TotalItemsCount),
                Filter = result.Filter
            };
        }
    }
}