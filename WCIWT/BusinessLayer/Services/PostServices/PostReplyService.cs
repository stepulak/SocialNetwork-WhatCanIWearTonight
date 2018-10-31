using BusinessLayer.DataTransferObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects.Filters;
using AutoMapper;
using WCIWT.Infrastructure;
using BusinessLayer.QueryObjects;

namespace BusinessLayer.Services.PostServices
{
    public class PostReplyService : CrudQueryServiceBase<PostReply, PostReplyDto, PostReplyFilterDto>
    {
        public PostReplyService(IMapper mapper, IRepository<PostReply> repository, PostReplyQueryObject query)
            : base(mapper, repository, query)
        {
        }

        protected override Task<PostReply> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "User", "Post");
        }
    }
}
