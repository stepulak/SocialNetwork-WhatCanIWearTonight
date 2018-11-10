using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.DataTransferObjects.Filters.Common;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Linq;
using System.Collections.Generic;
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

        public async Task<List<Guid>> ListOfFriends(Guid userId)
        {
            var friends = await ListOfPossibleFriends(userId);
            return friends.Where(t => t.Item2).Select(t => t.Item1).ToList();
        }

        public async Task<List<Guid>> ListOfFriendRequests(Guid userId)
        {
            var friends = await ListOfPossibleFriends(userId);
            return friends.Where(t => !t.Item2).Select(t => t.Item1).ToList();
        }

        private async Task<List<Tuple<Guid, bool>>> ListOfPossibleFriends(Guid userId)
        {
            var friendships = await ListFriendshipAsync(new FriendshipFilterDto { UserId = userId });
            var friends = new List<Tuple<Guid, bool>>();
            foreach (var friendship in friendships.Items)
            {
                if (!friendship.ApplicantId.HasValue || !friendship.RecipientId.HasValue)
                {
                    continue;
                }
                var applicantId = friendship.ApplicantId.Value;
                var recipientid = friendship.RecipientId.Value;
                friends.Add(new Tuple<Guid, bool>(applicantId == userId ? recipientid : applicantId, friendship.IsConfirmed));
            }
            return friends;
        }

        protected override Task<Friendship> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Applicant", "Recipient");
        }
    }
}

