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
using PresentationLayerMVC.Models.Aggregated;
using PresentationLayerMVC.Models.Friends;
using PresentationLayerMVC.Models.Messages;
using X.PagedList;

namespace PresentationLayerMVC.Controllers
{
    [RoutePrefix("messages")]
    public class MessageController : Controller
    {
        public UserFacade UserFacade { get; set; }
        public MessageFacade MessageFacade { get; set; }

        public const int MessagePageSize = 5;
        private const string FilterSessionKey = "filter";


        // GET: /messages/{username}
        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult> Index(string username, int page = 1)
        {
            var friend = await UserFacade.GetUserByUsernameAsync(username);
            var loggedUser = await GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var friendship = await UserFacade.GetFriendshipBetweenUsers(loggedUser.Id, friend.Id);
            if (friendship == null)
            {
                return RedirectToAction("Index", "User", new {username});
            }

            var model = new MessagesAggregatedViewModel
            {
                Friend = friend,
                MessagesListView = await GetMessagesOfUsers(loggedUser, friend, page),
                Page = page
            };
            return View("MessageView", model);
        }

        [HttpPost]
        [Route("{username}")]
        public async Task<ActionResult> SendMessage(string username, string NewMessageText)
        {
            if (String.IsNullOrWhiteSpace(NewMessageText))
            {
                //TODO: Return error;
                return await Index(username, 1);
            }

            var friend = await UserFacade.GetUserByUsernameAsync(username);
            var loggedUser = await GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var friendship = await UserFacade.GetFriendshipBetweenUsers(loggedUser.Id, friend.Id);
            if (friendship == null)
            {
                return RedirectToAction("Index", "User", new { username });
            }

            try
            {
                string url = Request.UrlReferrer.AbsolutePath;
                await MessageFacade.SendMessage(loggedUser, friend, NewMessageText);
                return Redirect(url);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Message", "Cannot send message to this user");
                return View();
            }
        }

        private async Task<MessageListViewModel> GetMessagesOfUsers(UserDto loggedUser, UserDto friend, int page)
        {
            var filter = Session[FilterSessionKey] as MessageFilterDto ?? new MessageFilterDto { PageSize = MessagePageSize };
            filter.RequestedPageNumber = page;

            var messages = await MessageFacade.MessagesBetweenUsers(loggedUser, friend, filter);
            return InitializeMessageListViewModel(messages);
        }

        private MessageListViewModel InitializeMessageListViewModel(QueryResultDto<MessageDto, MessageFilterDto> messages)
        {
            int page = messages.RequestedPageNumber ?? 1;
            messages.Items = messages.Items.OrderBy(message => message.Time);
            return new MessageListViewModel
            {
                Messages = new StaticPagedList<MessageDto>(
                    messages.Items,
                    page,
                    messages.PageSize,
                    (int)messages.TotalItemsCount),
                Filter = messages.Filter
            };
        }

        private async Task<UserDto> GetLoggedUser()
        {
            return HttpContext.User?.Identity != null
                ? await UserFacade.GetUserByUsernameAsync(HttpContext.User.Identity.Name)
                : null;
        }
    }
}