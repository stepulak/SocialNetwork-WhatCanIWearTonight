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
using BusinessLayer.Services.Friendships;
using BusinessLayer.Services.Hashtags;
using BusinessLayer.Services.PostServices;
using BusinessLayer.Services.Users;
using EntityDatabase;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.Posts
{
    public class PostService : CrudQueryServiceBase<Post, PostDto, PostFilterDto>, IPostService
    {
        private readonly IHashtagService hashtagService;
        private readonly IUserService userService;
        private readonly IFriendshipService friendshipService;

        public PostService(IMapper mapper, IRepository<Post> repository,
            QueryObjectBase<PostDto, Post, PostFilterDto, IQuery<Post>> postQueryObject,
            IHashtagService hashtagService,
            IUserService userService,
            IFriendshipService friendshipService)
            : base(mapper, repository, postQueryObject)
        {
            this.hashtagService = hashtagService;
            this.userService = userService;
            this.friendshipService = friendshipService;
        }

        public override Guid Create(PostDto entityDto)
        {
            return base.Create(entityDto);
        }

        public override async Task Update(PostDto entityDto)
        {
            await base.Update(entityDto);
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> ListPostAsync(PostFilterDto filter)
        {
            var query = await Query.ExecuteQuery(filter);
            /*if (filter.PostIdsWithHashtag != null)
            {
                query.Items = query.Items
                    .Where(i => filter.PostIdsWithHashtag.Contains(i.Id));
                query.TotalItemsCount = query.Items.LongCount();
            }*/
            return query;
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> ListUserOwnedPosts(Guid userId)
        {
            return await ListPostAsync(new PostFilterDto { UserId = userId });
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> ListPostsAvailableForUser(Guid loggedUserId,
            PostFilterDto filter)
        {
            filter.IncludePrivatePosts = true;
            var user = await userService.GetAsync(loggedUserId);
            var userFriends = await friendshipService.ListOfFriendsAsync(loggedUserId);
            var userFriendsIds = userFriends.Select(friend => friend.Id).ToList();
            var userAge = (int)((DateTime.Now - user.Birthdate).TotalDays / 365.2425);
            filter.LoggedUserId = loggedUserId;
            filter.UserAge = userAge;
            filter.GenderRestriction = user.Gender;
            filter.PostUserIds = userFriendsIds;
            filter.PostUserIds.Add(loggedUserId);
           return await ListPostAsync(filter);
        }
        
        protected override Task<Post> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "User");
        }
    }
}
