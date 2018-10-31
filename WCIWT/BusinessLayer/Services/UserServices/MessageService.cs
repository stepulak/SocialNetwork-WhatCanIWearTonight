using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using WCIWT.Infrastructure;

namespace BusinessLayer.Services.UserServices
{
    public class MessageService : CrudQueryServiceBase<Message, MessageDto, MessageFilterDto>
    {
        public MessageService(IMapper mapper, IRepository<Message> repository, MessageQueryObject query)
            : base(mapper, repository, query)
        {
        }

        protected override Task<Message> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "UserSender", "UserReceiver");
        }
    }
}
