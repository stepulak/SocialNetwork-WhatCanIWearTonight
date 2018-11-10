using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using BusinessLayer.QueryObjects.Common;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.UserServices
{
    public class MessageService : CrudQueryServiceBase<Message, MessageDto, MessageFilterDto>, IMessageService
    {
        private readonly QueryObjectBase<MessageDto, Message, MessageFilterDto, IQuery<Message>> messageQueryObject;

        public MessageService(IMapper mapper, IRepository<Message> repository, MessageQueryObject query,
            QueryObjectBase<MessageDto, Message, MessageFilterDto, IQuery<Message>> messageQueryObject)
            : base(mapper, repository, query)
        {
            this.messageQueryObject = messageQueryObject;
        }

        protected override Task<Message> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "UserSender", "UserReceiver");
        }

        public async Task<QueryResultDto<MessageDto, MessageFilterDto>> ListMessageAsync(MessageFilterDto filter)
        {
            return await messageQueryObject.ExecuteQuery(filter);
        }
    }
}
