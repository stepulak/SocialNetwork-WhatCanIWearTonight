﻿using System;
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
            var postId = base.Create(entityDto);
            AddHashtagsToPost(entityDto);
            return postId;
        }

        public override async Task Update(PostDto entityDto)
        {
            await base.Update(entityDto);
            await RemoveHashtagsForPost(entityDto);
            AddHashtagsToPost(entityDto);
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> ListPostAsync(PostFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> ListUserOwnedPosts(Guid userId)
        {
            return await ListPostAsync(new PostFilterDto { UserId = userId });
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> ListPostsAvailableForUser(Guid userId,
            PostFilterDto filter)
        {
            UserDto user = await userService.GetAsync(userId);
            List<UserDto> userFriends = await friendshipService.ListOfFriendsAsync(userId);
            List<Guid> userFriendsIds = userFriends.Select(x => x.Id).ToList();
            var userAge = (int)((DateTime.Now - user.Birthdate).TotalDays / 365.2425);
            filter.UserAge = userAge;
            filter.GenderRestriction = user.Gender;

            var allPosts = await ListPostAsync(filter);
            // TODO: filterDTO should be rewritten to allow needed filtering
            // Filter all posts who are private and you are not friend of post's owner.
            allPosts.Items = allPosts.Items
                .Where(post => userFriendsIds.Contains(post.UserId)
                               || post.UserId == userId
                               || post.Visibility == DataTransferObjects.PostVisibility.Public);
            return allPosts;
        }
        
        protected override Task<Post> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "User");
        }

        private static List<string> FindHashtags(string text)
        {
            var hashtags = new List<string>();
            var index = text.IndexOf('#');
            while (index >= 0)
            {
                int tagEnd = index;
                while (tagEnd < text.Length && !char.IsWhiteSpace(text[tagEnd])) { tagEnd++; }
                hashtags.Add(text.Substring(index, tagEnd - index));
                index = text.Substring(index).IndexOf('#');
            }
            return hashtags;
        }

        private void AddHashtagsToPost(PostDto postDto)
        {
            var hashtags = FindHashtags(postDto.Text);
            foreach (var hashtag in hashtags)
            {
                hashtagService.Create(new HashtagDto { Tag = hashtag, PostId = postDto.Id });
            }
        }

        private async Task RemoveHashtagsForPost(PostDto postDto)
        {
            var oldHashtags = await hashtagService.ListHashtagAsync(new HashtagFilterDto { PostId = postDto.Id });
            foreach (var hashtag in oldHashtags.Items)
            {
                hashtagService.Delete(hashtag.Id);
            }
        }
    }
}
