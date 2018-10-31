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
        public PostReplyService(IMapper mapper, IRepository<PostReply> postRepository, PostReplyQueryObject query)
            : base(mapper, postRepository, query)
        {
        }

        protected override Task<PostReply> GetWithIncludesAsync(Guid entityId)
        {
            var userTableName = new User().TableName;
            var postTableName = new Post().TableName;
            return Repository.GetAsync(entityId, new string[] { userTableName, postTableName });
        }
    }
}
