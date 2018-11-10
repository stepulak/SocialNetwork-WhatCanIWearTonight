using BusinessLayer.DataTransferObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects.Filters;
using AutoMapper;
using WCIWT.Infrastructure;
using BusinessLayer.DataTransferObjects.Filters;
using WCIWT.Infrastructure.Query;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.QueryObjects.Common;

namespace BusinessLayer.Services.PostServices
{
    public class PostReplyService : CrudQueryServiceBase<PostReply, PostReplyDto, PostReplyFilterDto>, IPostReplyService
    {
        private readonly QueryObjectBase<PostReplyDto, PostReply, PostReplyFilterDto, IQuery<PostReply>> postReplyQueryObject;

        public PostReplyService(IMapper mapper, IRepository<PostReply> repository, PostReplyQueryObject query,
             QueryObjectBase<PostReplyDto, PostReply, PostReplyFilterDto, IQuery<PostReply>> postReplyQueryObject)
            : base(mapper, repository, query)
        {
            this.postReplyQueryObject = postReplyQueryObject;
        }

        public async Task<QueryResultDto<PostReplyDto, PostReplyFilterDto>> ListPostReplyAsync(PostReplyFilterDto filter)
        {
            return await postReplyQueryObject.ExecuteQuery(filter);
        }

        protected override Task<PostReply> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "User", "Post");
        }
    }
}
