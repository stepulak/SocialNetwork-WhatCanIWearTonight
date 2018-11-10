using BusinessLayer.DataTransferObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects.Filters;
using AutoMapper;
using WCIWT.Infrastructure;
using BusinessLayer.DataTransferObjects.Filters.Common;
using WCIWT.Infrastructure.Query;
using BusinessLayer.DataTransferObjects.Common;
using System.Collections.Generic;

namespace BusinessLayer.Services.PostServices
{
    public class PostService : CrudQueryServiceBase<Post, PostDto, PostFilterDto>, IPostService
    {
        private readonly QueryObjectBase<PostDto, Post, PostFilterDto, IQuery<Post>> postQueryObject;
        private readonly HashtagService hashtagService;

        public PostService(IMapper mapper, IRepository<Post> repository, PostQueryObject query,
            QueryObjectBase<PostDto, Post, PostFilterDto, IQuery<Post>> postQueryObject,
            HashtagService hashtagService)
            : base(mapper, repository, query)
        {
            this.postQueryObject = postQueryObject;
            this.hashtagService = hashtagService;
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
            return await postQueryObject.ExecuteQuery(filter);
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> ListUserOwnedPosts(Guid userId)
        {
            return await ListPostAsync(new PostFilterDto { UserId = userId });
        }

        public async Task<List<PostDto>> ListPostsAvailableForUser(UserDto user, List<Guid> userFriends)
        {
            var userAge = Convert.ToDateTime(DateTime.Now - user.Birthdate).Year;
            var allPosts = await ListPostAsync(new PostFilterDto
            {
                UserAge = userAge,
                GenderRestriction = user.Gender
            });
            var posts = new List<PostDto>();
            // Filter all posts who are private and you are not friend of post's owner.
            foreach (var post in allPosts.Items)
            {
                if (post.Visibility != DataTransferObjects.PostVisibility.FriendsOnly || userFriends.Contains(post.UserId.Value))
                {
                    continue;
                }
                posts.Add(post);
            }
            return posts;
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
