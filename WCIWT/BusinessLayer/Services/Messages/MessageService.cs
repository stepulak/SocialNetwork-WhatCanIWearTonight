using System;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Common;
using BusinessLayer.Services.Common;
using EntityDatabase;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.Messages
{
    public class MessageService : CrudQueryServiceBase<Message, MessageDto, MessageFilterDto>, IMessageService
    {

        public MessageService(IMapper mapper, IRepository<Message> repository,
            QueryObjectBase<MessageDto, Message, MessageFilterDto, IQuery<Message>> messageQueryObject)
            : base(mapper, repository, messageQueryObject)
        {
        }

        protected override Task<Message> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "UserSender", "UserReceiver");
        }

        public async Task<QueryResultDto<MessageDto, MessageFilterDto>> ListMessageAsync(MessageFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }
    }
}
