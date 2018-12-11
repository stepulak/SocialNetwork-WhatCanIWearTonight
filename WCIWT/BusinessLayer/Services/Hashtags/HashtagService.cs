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
        public HashtagService(IMapper mapper, IRepository<Hashtag> repository, QueryObjectBase<HashtagDto, Hashtag, HashtagFilterDto, IQuery<Hashtag>> hashtagQueryObject)
            : base(mapper, repository, hashtagQueryObject)
        {
        }

        public async Task<QueryResultDto<HashtagDto, HashtagFilterDto>> ListHashtagAsync(HashtagFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }
        
        protected override Task<Hashtag> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Post");
        }
    }
}