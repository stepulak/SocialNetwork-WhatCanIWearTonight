using System;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Common;
using BusinessLayer.Services.Common;
using BusinessLayer.Services.PostServices;
using EntityDatabase;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.PostReplys
{
    public class PostReplyService : CrudQueryServiceBase<PostReply, PostReplyDto, PostReplyFilterDto>, IPostReplyService
    {

        public PostReplyService(IMapper mapper, IRepository<PostReply> repository,
             QueryObjectBase<PostReplyDto, PostReply, PostReplyFilterDto, IQuery<PostReply>> postReplyQueryObject)
            : base(mapper, repository, postReplyQueryObject)
        {
        }

        public async Task<QueryResultDto<PostReplyDto, PostReplyFilterDto>> ListPostReplyAsync(PostReplyFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }

        protected override Task<PostReply> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "User", "Post");
        }
    }
}
