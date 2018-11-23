using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.QueryObjects.Common;
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

        public async Task<List<UserDto>> ListOfFriendsAsync(Guid userId)
        {
            var friends = await ListOfPossibleFriendsAsync(userId);
            return friends.Where(t => t.Item2).Select(t => t.Item1).ToList();
        }

        public async Task<List<UserDto>> ListOfFriendRequestsAsync(Guid userId)
        {
            var friends = await ListOfPossibleFriendsAsync(userId);
            return friends.Where(t => !t.Item2).Select(t => t.Item1).ToList();
        }

        private async Task<List<Tuple<UserDto, bool>>> ListOfPossibleFriendsAsync(Guid userId)
        {
            var friendships = await ListFriendshipAsync(new FriendshipFilterDto { UserA = userId });
            return (from friendship in friendships.Items
                let applicantId = friendship.ApplicantId
                select new Tuple<UserDto, bool>(applicantId == userId ? friendship.Recipient : friendship.Applicant,
                    friendship.IsConfirmed)).ToList();
        }

        protected override Task<Friendship> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Applicant", "Recipient");
        }
    }
}

