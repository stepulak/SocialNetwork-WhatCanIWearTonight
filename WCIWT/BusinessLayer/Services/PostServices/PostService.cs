using BusinessLayer.DataTransferObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects.Filters;
using AutoMapper;
using WCIWT.Infrastructure;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Common;
using WCIWT.Infrastructure.Query;
using BusinessLayer.DataTransferObjects.Common;

namespace BusinessLayer.Services.PostServices
{
    public class PostService : CrudQueryServiceBase<Post, PostDto, PostFilterDto>, IPostService
    {
        private readonly QueryObjectBase<PostDto, Post, PostFilterDto, IQuery<Post>> postQueryObject;

        public PostService(IMapper mapper, IRepository<Post> repository, PostQueryObject query,
            QueryObjectBase<PostDto, Post, PostFilterDto, IQuery<Post>> postQueryObject)
            : base(mapper, repository, query)
        {
            this.postQueryObject = postQueryObject;
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> ListPostAsync(PostFilterDto filter)
        {
            return await postQueryObject.ExecuteQuery(filter);
        }

        protected override Task<Post> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "User");
        }
    }
}
