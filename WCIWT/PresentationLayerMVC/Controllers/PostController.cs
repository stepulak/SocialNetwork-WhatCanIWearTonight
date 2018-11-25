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
    public class PostController : Controller
    {
        public PostFacade PostFacade { get; set; }

        public ActionResult Index()
        {
            return View();
        }

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
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult AddComment(Guid postId, Guid userId, string comment)
        {
            if (postId == Guid.Empty || userId == Guid.Empty)
            {
                return Index();
            }
            try
            {
                PostFacade.CommentPost(postId, userId, comment);
                return View("Index", "Post");
            }
            catch(Exception e)
            {
                ModelState.AddModelError("Comment", e.Message);
                return View();
            }
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
                await PostFacade.AddVote(imageId, userId, type);
                return View("Index", "Post");
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
                await PostFacade.RemoveVote(imageId, userId);
                return View("Index", "Post");
            }
            catch(Exception)
            {
                ModelState.AddModelError("Image", "Image does not exist!");
                return View();
            }
        }

    }
}