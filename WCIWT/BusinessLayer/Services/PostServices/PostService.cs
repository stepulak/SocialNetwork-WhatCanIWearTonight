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
    public class PostService : CrudQueryServiceBase<Post, PostDto, PostFilterDto>
    {
        public PostService(IMapper mapper, IRepository<Post> repository, PostQueryObject query)
            : base(mapper, repository, query)
        {
        }

        protected override Task<Post> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "User");
        }
    }
}
