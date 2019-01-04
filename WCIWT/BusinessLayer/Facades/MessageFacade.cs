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
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.Facades.Common;
using BusinessLayer.Services.Messages;
using BusinessLayer.Services.Users;
using WCIWT.Infrastructure.UnitOfWork;

namespace BusinessLayer.Facades
{
    public class MessageFacade : FacadeBase
    {
        private readonly IMessageService messageService;
        private readonly IUserService userService;

        public MessageFacade(IUnitOfWorkProvider unitOfWorkProvider,
            IMessageService messageService, IUserService userService)
            : base(unitOfWorkProvider)
        {
            this.messageService = messageService;
            this.userService = userService;
        }

        public async Task MarkAsRead(MessageDto message)
        {
            message.Seen = true;
            using (var uow = UnitOfWorkProvider.Create())
            {

                await messageService.Update(message);
                await uow.Commit();
            }
        }

        public async Task<long> NumberOfUnreadMessages(UserDto user) 
            => (await UnreadMessages(user, new MessageFilterDto())).TotalItemsCount;
        
        public async Task<QueryResultDto<MessageDto, MessageFilterDto>> UnreadMessages(UserDto user, MessageFilterDto filter)
        {
            filter.SortCriteria = "Time";
            filter.Sender = user.Id;
            filter.CareAboutRole = true;
            filter.UnseenOnly = true;
            using (UnitOfWorkProvider.Create())
            {
                return await messageService.ListMessageAsync(filter);
            }
        }

        public async Task<QueryResultDto<MessageDto, MessageFilterDto>> ReceivedMessages(UserDto user, MessageFilterDto filter)
        {
            filter.SortCriteria = "Time";
            filter.Receiver = user.Id;
            filter.CareAboutRole = true;
            using (UnitOfWorkProvider.Create())
            {
                return await messageService.ListMessageAsync(filter);
            }
        }
        public async Task<QueryResultDto<MessageDto, MessageFilterDto>> SentMessages(UserDto user, MessageFilterDto filter)
        {
            filter.SortCriteria = "Time";
            filter.Sender = user.Id;
            filter.CareAboutRole = true;
            using (UnitOfWorkProvider.Create())
            {
                return await messageService.ListMessageAsync(filter);
            }
        }

        public async Task<QueryResultDto<MessageDto, MessageFilterDto>> AllMessages(Guid userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await messageService.ListMessageAsync(new MessageFilterDto { CareAboutRole = false, Sender = userId });
            }
        }

        public async Task<Guid> SendMessage(UserDto sender, UserDto receiver, string text)
        {
            var message = new MessageDto
            {
                Seen = false,
                Text = text,
                Time = DateTime.Now,
                UserSenderId = sender.Id,
                UserReceiverId = receiver.Id
            };
            using (var uow = UnitOfWorkProvider.Create())
            {
                var id = messageService.Create(message);
                await uow.Commit();
                return id;
            }
        }

        public async Task DeleteMessage(Guid messageId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                messageService.Delete(messageId);
                await uow.Commit();
            }
        }

        public async Task<QueryResultDto<MessageDto, MessageFilterDto>> MessagesBetweenUsers(UserDto userA, UserDto userB, MessageFilterDto filter)
        {
            filter.SortCriteria = "Time";
            filter.SortAscending = true;
            filter.Sender = userA.Id;
            filter.Receiver = userB.Id;
            filter.CareAboutRole = false;
            using (UnitOfWorkProvider.Create())
            {
                return await messageService.ListMessageAsync(filter);
;            }
        }
    }
}
