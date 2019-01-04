using BusinessLayer.DataTransferObjects;
using BusinessLayer.Facades;
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
                return RedirectToAction("Login", "Account");
            }
            catch(ArgumentException)
            {
                ModelState.AddModelError("Username", "Account with that username already exists!");
                return View();
            }
        }
        
        [HttpGet]
        [Route("login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (await UserFacade.Login(model.Username, model.Password))
            {
                SetupLoginCookie(model.Username, await UserFacade.IsAdmin(model.Username));

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

        [HttpGet]
        [Route("logout")]
        public RedirectToRouteResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        private void SetupLoginCookie(string username, bool isAdmin)
        {
            var ticket = new FormsAuthenticationTicket(
                1,                                     // ticket version
                username,                              // authenticated username
                DateTime.Now,                          // issueDate
                DateTime.Now.AddMinutes(30),           // expiryDate
                false,                          // true to persist across browser sessions
                isAdmin ? "admin" : "",                              // can be used to store additional user data
                FormsAuthentication.FormsCookiePath);  // the path for the cookie

            // Encrypt the ticket using the machine key
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Add the cookie to the request to save it
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true
            };
            Response.Cookies.Add(cookie);
        }
    }
}