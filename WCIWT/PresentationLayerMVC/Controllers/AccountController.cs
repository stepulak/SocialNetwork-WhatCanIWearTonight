using BusinessLayer.DataTransferObjects;
using BusinessLayer.Facades.Common;
using PresentationLayerMVC.Models.Accounts;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DemoEshop.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        public UserFacade UserFacade { get; set; }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserCreateDto userCreateDto)
        {
            try
            {
                await UserFacade.RegisterUser(userCreateDto);
                //FormsAuthentication.SetAuthCookie(userCreateDto.Username, false);
                
                var authTicket = new FormsAuthenticationTicket(1, userCreateDto.Username, DateTime.Now,
                    DateTime.Now.AddMinutes(30), false, "");
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");
            }
            catch(ArgumentException)
            {
                ModelState.AddModelError("Username", "Account with that username already exists!");
                return View();
            }
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (await UserFacade.Login(model.Username, model.Password))
            {
                //FormsAuthentication.SetAuthCookie(model.Username, false);

                var authTicket = new FormsAuthenticationTicket(1, model.Username, DateTime.Now,
                    DateTime.Now.AddMinutes(30), false, null);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                var decodedUrl = "";
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    decodedUrl = Server.UrlDecode(returnUrl);
                }

                if (Url.IsLocalUrl(decodedUrl))
                {
                    return Redirect(decodedUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Wrong username or password!");
            return View();
        }

        public Task<ActionResult> Logout()
        {
            FormsAuthentication.SignOut();
            return new Task<ActionResult>(() => RedirectToAction("Index", "Home"));
        }
    }
}