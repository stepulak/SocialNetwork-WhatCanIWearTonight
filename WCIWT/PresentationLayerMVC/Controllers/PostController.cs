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
            var model = new PostWithReplysViewModel
            {
                Post = post,
                Images = new StaticPagedList<ImageDto>(images, 1, images.Count, images.Count),
                Replys = new StaticPagedList<PostReplyDto>(replys, 1, replys.Count, replys.Count)
            };
            return View("Post", model);
        }

        [HttpPost]
        [Route("comment")]
        public async Task<ActionResult> AddComment(PostWithReplysViewModel model)
        {
            return await AddComment(Guid.Parse(model.PostId), model.Username, model.TextComment);
        }
        
        [HttpPost]
        public async Task<ActionResult> Vote(Guid imageId, Guid userId, VoteType type)
        {
            if (imageId == Guid.Empty || userId == Guid.Empty)
            {
                return Index();
            }
            try
            {
                string url = Request.UrlReferrer.AbsolutePath;
                await PostFacade.AddVote(imageId, userId, type);
                return Redirect(url);
            }
            catch(Exception)
            {
                ModelState.AddModelError("Image", "Image does not exist!");
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveVote(Guid imageId, Guid userId)
        {
            if (imageId == Guid.Empty || userId == Guid.Empty)
            {
                return Index();
            }
            try
            {
                string url = Request.UrlReferrer.AbsolutePath;
                await PostFacade.RemoveVote(imageId, userId);
                return Redirect(url);
            }
            catch(Exception)
            {
                ModelState.AddModelError("Image", "Image does not exist!");
                return View();
            }
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
                PostFacade.CommentPost(postId, userId, comment);
                return Redirect(url);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Comment", e.Message);
                return View();
            }
        }
    }
}