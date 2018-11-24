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

namespace BusinessLayer.Services.Hashtags
{
    public class HashtagService : CrudQueryServiceBase<Hashtag, HashtagDto, HashtagFilterDto>, IHashtagService
    {
        private readonly QueryObjectBase<HashtagDto, Hashtag, HashtagFilterDto, IQuery<Hashtag>> HashtagQueryObject;

        public HashtagService(IMapper mapper, IRepository<Hashtag> repository, HashtagQueryObject query,
            QueryObjectBase<HashtagDto, Hashtag, HashtagFilterDto, IQuery<Hashtag>> hashtagQueryObject)
            : base(mapper, repository, query)
        {
            this.HashtagQueryObject = hashtagQueryObject;
        }
        
        protected override Task<Hashtag> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Post");
        }

        public async Task<QueryResultDto<HashtagDto, HashtagFilterDto>> ListHashtagAsync(HashtagFilterDto filter)
        {
            return await HashtagQueryObject.ExecuteQuery(filter);
        }
    }
}