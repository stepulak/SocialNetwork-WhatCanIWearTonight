using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Facades;
using BusinessLayer.Services.PostServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Facades.Common;
using BusinessLayer.Services.Messages;
using BusinessLayer.Services.Users;
using WCIWT.Infrastructure.UnitOfWork;

namespace BusinessLayer.Facades
{
    public class MessageFacade : FacadeBase
    {
        private readonly MessageService messageService;
        private readonly UserService userService;

        public MessageFacade(IUnitOfWorkProvider unitOfWorkProvider,
            MessageService messageService, UserService userService)
            : base(unitOfWorkProvider)
        {
            this.messageService = messageService;
            this.userService = userService;
        }

        public async Task<int> NumberOfUnreadMessages(UserDto user) 
            => (await UnreadMessages(user)).Count;
        
        public async Task<List<MessageDto>> UnreadMessages(UserDto user)
        {
            var result = await ReceivedMessages(user);
            return result.Where(m => !m.Seen).ToList();
        }

        public async Task<List<MessageDto>> ReceivedMessages(UserDto user) 
            => await AllMessages(user, MessageUserFilterType.Receiver);
        
        public async Task<List<MessageDto>> SentMessages(UserDto user) 
            => await AllMessages(user, MessageUserFilterType.Sender);
        
        public async Task ReplyToMessage(MessageDto message, string text)
        {
            var builder = new StringBuilder(text + "\n\n");
            using (var reader = new StringReader(message.Text))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        builder.Append($"> {line}");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            var sender = await userService.GetAsync(message.UserReceiverId);
            var receiver = await userService.GetAsync(message.UserSenderId);
            SendMessage(sender, receiver, builder.ToString());
        }

        public Guid SendMessage(UserDto sender, UserDto receiver, string text)
        {
            var message = new MessageDto
            {
                Seen = false,
                Text = text,
                Time = DateTime.Now,
                UserSenderId = sender.Id,
                UserReceiverId = sender.Id
            };
            return messageService.Create(message);
        }

        public void DeleteMessage(MessageDto message) => messageService.Delete(message.Id);
        
        private async Task<List<MessageDto>> AllMessages(UserDto user, MessageUserFilterType filterType)
        {
            var filter = new MessageFilterDto
            {
                UserId = user.Id,
                UserFilterType = filterType
            };
            var result = await messageService.ListMessageAsync(filter);
            return result.Items.OrderByDescending(m => m.Time).ToList();
        }
    }
}
