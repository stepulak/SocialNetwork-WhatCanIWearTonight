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

        [HttpPost]
        [Route("new")]
        public async Task<ActionResult> NewPost(CreatePostModel model)
        {
            var user = await GetLoggedUser();
            if (user != null)
            {
                var newPostId =  await PostFacade.AddPost(user, model.Post);
                var newPost = await PostFacade.GetPostDtoAccordingToId(newPostId);
                var uploadedSuccessfully = await CreateImageDtosFromFiles(model.Files, newPost);
                if (uploadedSuccessfully)
                {
                    RedirectToAction("Index", "Post", new { postId = newPostId });
                }
            }
            else
            {
                RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index", "Home");
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