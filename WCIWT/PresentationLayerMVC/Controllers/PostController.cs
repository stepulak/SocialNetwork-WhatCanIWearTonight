using BusinessLayer.DataTransferObjects;
using BusinessLayer.Facades;
using PresentationLayerMVC.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace PresentationLayerMVC.Controllers
{
    [RoutePrefix("posts")]
    public class PostController : Controller
    {
        public UserFacade UserFacade { get; set; }
        public PostFacade PostFacade { get; set; }
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult> Index(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                return Index();
            }
            var post = await PostFacade.GetPostDtoAccordingToId(postId);
            var replys = await PostFacade.ListOfReplysForPost(postId);
            var images = await PostFacade.ListOfImagesForPost(postId);
            var model = new PostModel
            {
                Post = post,
                Images = new StaticPagedList<ImageDto>(images, 1, images.Count, images.Count),
                Replys = new StaticPagedList<PostReplyDto>(replys, 1, replys.Count, replys.Count)
            };
            return View("Post", model);
        }

        [HttpPost]
        [Route("comment")]
        public async Task<ActionResult> AddComment(PostModel model)
        {
            return await AddComment(Guid.Parse(model.PostId), model.Username, model.TextComment);
        }
        
        //[HttpGet]
        [Route("like/{username}/{imageId}")]
        public async Task<ActionResult> Like(string username, Guid imageId)
        {
            var userId = (await UserFacade.GetUserByUsernameAsync(username)).Id;
            return await Vote(imageId, userId, VoteType.Like);
        }

        //[HttpGet]
        [Route("dislike/{username}/{imageId}")]
        public async Task<ActionResult> Dislike(string username, Guid imageId)
        {
            var userId = (await UserFacade.GetUserByUsernameAsync(username)).Id;
            return await Vote(imageId, userId, VoteType.Dislike);
        }

        [HttpGet]
        [Route("new")]
        public ActionResult NewPost()
        {
            return View();
        }

        private async Task<ActionResult> AddComment(Guid postId, string username, string comment)
        {
            if (postId == Guid.Empty)
            {
                return Index();
            }
            try
            {
                string url = Request.UrlReferrer.AbsolutePath;
                var userId = (await UserFacade.GetUserByUsernameAsync(username)).Id;
                await PostFacade.CommentPost(postId, userId, comment);
                return Redirect(url);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Comment", e.Message);
                return View();
            }
        }

        private async Task<ActionResult> Vote(Guid imageId, Guid userId, VoteType type)
        {
            if (imageId == Guid.Empty || userId == Guid.Empty)
            {
                return Index();
            }
            try
            {
                string url = Request.UrlReferrer.AbsolutePath;
                await PostFacade.ChangeVote(imageId, userId, type);
                return Redirect(url);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Image", "Image does not exist!");
                return View();
            }
        }
    }
}