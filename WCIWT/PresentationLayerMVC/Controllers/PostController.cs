using BusinessLayer.DataTransferObjects;
using BusinessLayer.Facades;
using PresentationLayerMVC.Models.Posts;
using System;
using System.Collections.Generic;
using System.IO;
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
                Replys = new StaticPagedList<PostReplyDto>(replys, 1, replys.Count, replys.Count),
                HashtagIndices = PostFacade.FindHashtagIndices(post.Text)
            };
            return View("Post", model);
        }

        [HttpPost]
        [Route("reply")]
        public async Task<ActionResult> AddReply(PostModel model)
        {
            return await AddReply(Guid.Parse(model.PostId), model.Username, model.TextComment);
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

        [HttpPost]
        [Route("new")]
        public async Task<ActionResult> NewPost(CreatePostModel model)
        {
            if (model.Post.HasAgeRestriction && (model.Post.AgeRestrictionFrom == null || model.Post.AgeRestrictionFrom <= 0))
            {
                ModelState.AddModelError("Post.AgeRestrictionFrom", "Age Restriction From must be larger or equal to 0");
                return View();
            }

            if (model.Post.HasAgeRestriction && (model.Post.AgeRestrictionTo == null || model.Post.AgeRestrictionTo <= 0))
            {
                ModelState.AddModelError("Post.AgeRestrictionTo", "Age Restriction To must be larger or equal to 0");
                return View();
            }

            if (model.Post.HasAgeRestriction && model.Post.AgeRestrictionFrom > model.Post.AgeRestrictionTo)
            {
                ModelState.AddModelError("Post.AgeRestrictionFrom", "Age Restriction From must be smaller than Age Restriction To");
                ModelState.AddModelError("Post.AgeRestrictionTo", "Age Restriction To must be larger than Age Restriction From");
                return View();
            }
            var user = await GetLoggedUser();
            if (user != null)
            {
                var newPostId = await PostFacade.AddPost(user, model.Post);
                var newPost = await PostFacade.GetPostDtoAccordingToId(newPostId);
                var uploadedSuccessfully = await CreateImageDtosFromFiles(model.Files, newPost);
                if (uploadedSuccessfully)
                {
                    return RedirectToAction("Index", "Post", new { postId = newPostId });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpGet]
        [Route("remove-post/{postId}")]
        public async Task<ActionResult> RemovePost(Guid postId)
        {
            var user = await GetLoggedUser();
            var post = await PostFacade.GetPostDtoAccordingToId(postId);
            if (user != null && post != null && (user.IsAdmin || post.UserId == user.Id))
            {
                await PostFacade.DeletePost(postId);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("remove-reply/{replyId}")]
        public async Task<ActionResult> RemoveReply(Guid replyId)
        {
            var user = await GetLoggedUser();
            var reply = await PostFacade.GetReplyById(replyId);
            string url = Request.UrlReferrer.AbsoluteUri;
            if (user != null && reply != null && (user.IsAdmin || reply.UserId == user.Id))
            {
                await PostFacade.DeleteReply(replyId);
            }
            return Redirect(url);
        }

        private async Task<ActionResult> AddReply(Guid postId, string username, string reply)
        {
            if (postId == Guid.Empty)   
            {
                return Index();
            }
            try
            {
                string url = Request.UrlReferrer.AbsoluteUri;
                var userId = (await UserFacade.GetUserByUsernameAsync(username)).Id;
                await PostFacade.AddReplyToPost(postId, userId, reply);
                return Redirect(url);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Reply", e.Message);
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
                string url = Request.UrlReferrer.AbsoluteUri;
                await PostFacade.ChangeVote(imageId, userId, type);
                return Redirect(url);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Image", "Image does not exist!");
                return View();
            }
        }

        private async Task<bool> CreateImageDtosFromFiles(List<HttpPostedFileBase> files, PostDto post)
        {
            foreach (var file in files)
            {
                var image = new ImageCreateDto
                {
                    BinaryImage = createByteImage(file),
                    PostId = post.Id
                };
                var imageId = await PostFacade.AddImage(image);
                if (imageId == Guid.Empty)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<UserDto> GetLoggedUser()
        {
            return HttpContext.User?.Identity != null
                ? await UserFacade.GetUserByUsernameAsync(HttpContext.User.Identity.Name)
                : null;
        }

        private byte[] createByteImage(HttpPostedFileBase file)
        {
            string imagesPath = HttpContext.Server.MapPath("~/Content/TempImages"); // Or file save folder, etc.
            string extension = Path.GetExtension(file.FileName);
            string newFileName = $"NewFile{extension}";
            string saveToPath = Path.Combine(imagesPath, newFileName);
            file.SaveAs(saveToPath);
            var imageBytes = System.IO.File.ReadAllBytes(saveToPath);
            System.IO.File.Delete(saveToPath);
            return imageBytes;
        }
    }
}