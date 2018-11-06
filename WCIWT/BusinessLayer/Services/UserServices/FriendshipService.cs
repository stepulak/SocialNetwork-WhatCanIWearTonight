using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.DataTransferObjects.Filters.Common;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.UserServices
{
    public class FriendshipService : CrudQueryServiceBase<Friendship, FriendshipDto, FriendshipFilterDto>, IFriendshipService
    {
        private readonly QueryObjectBase<FriendshipDto, Friendship, FriendshipFilterDto, IQuery<Friendship>> friendshipQueryObject;

        public FriendshipService(IMapper mapper, IRepository<Friendship> repository, FriendshipQueryObject query,
             QueryObjectBase<FriendshipDto, Friendship, FriendshipFilterDto, IQuery<Friendship>> friendshipQueryObject)
            : base(mapper, repository, query)
        {
            this.friendshipQueryObject = friendshipQueryObject;
        }

        public async Task<QueryResultDto<FriendshipDto, FriendshipFilterDto>> ListFriendshipAsync(FriendshipFilterDto filter)
        {
            return await friendshipQueryObject.ExecuteQuery(filter);
        }

        protected override Task<Friendship> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Applicant", "Recipient");
        }
    }
}

