using BusinessLayer.Facades;
using PresentationLayerMVC.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayerMVC.Controllers
{
    [RoutePrefix("users")]
    public class UserProfileController : Controller
    {
        public UserFacade UserFacade { get; set; }

        // GET: user/{username}
        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult> Index(string username)
        {
            var user = await UserFacade.GetUserByUsernameAsync(username);
            var model = new UserProfileViewModel()
            {
                User = user
            };
            return View($"UserProfileView", model);
        }
    }
}