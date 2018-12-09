using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Common;
using BusinessLayer.Services.Common;
using EntityDatabase;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.Friendships
{
    public class FriendshipService : CrudQueryServiceBase<Friendship, FriendshipDto, FriendshipFilterDto>, IFriendshipService
    {
        public FriendshipService(IMapper mapper, IRepository<Friendship> repository,
            QueryObjectBase<FriendshipDto, Friendship, FriendshipFilterDto, IQuery<Friendship>> friendshipQuery)
            : base(mapper, repository, friendshipQuery)
        {
        }
        
        public async Task<QueryResultDto<FriendshipDto, FriendshipFilterDto>> ListFriendshipAsync(FriendshipFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }

        public async Task<QueryResultDto<UserDto, FriendshipFilterDto>> GetFriendsOfUserAsync(Guid userId, FriendshipFilterDto filter)
        {
            var friendships = await ListFriendshipAsync(filter);
            var recipientFriends = friendships.Items
                .Where(friendship => friendship.ApplicantId == userId)
                .Select(friendship => friendship.Recipient);

            var applicantFriends = friendships.Items
                .Where(friendship => friendship.RecipientId == userId)
                .Select(friendship => friendship.Applicant);
            return new QueryResultDto<UserDto, FriendshipFilterDto>()
            {
                Items = recipientFriends.Union(applicantFriends),
                TotalItemsCount = friendships.TotalItemsCount,
                PageSize = friendships.PageSize,
                RequestedPageNumber = friendships.RequestedPageNumber,
                Filter = filter
            };
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

