using BusinessLayer.DataTransferObjects;
using BusinessLayer.Facades;
using BusinessLayer.Services.PostServices;
using BusinessLayer.DataTransferObjects.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.Facades.Common;
using BusinessLayer.Services.Images;
using BusinessLayer.Services.PostReplys;
using BusinessLayer.Services.Posts;
using BusinessLayer.Services.Votes;
using WCIWT.Infrastructure.UnitOfWork;
using BusinessLayer.Services.Friendships;

namespace BusinessLayer.Facades
{
    public class PostFacade : FacadeBase
    {
        public const int MinimalPostReplyLength = 2;

        private readonly IPostService postService;
        private readonly IFriendshipService friendshipService;
        private readonly IVoteService voteService;
        private readonly IImageService imageService;
        private readonly IPostReplyService postReplyService;
        private readonly IHashtagService hashtagService;

        public PostFacade(IUnitOfWorkProvider unitOfWorkProvider, 
            IPostService postService, IPostReplyService postReplyService, IFriendshipService friendshipService,
            IVoteService voteService, IImageService imageService,
            IHashtagService hashtagService)
            : base(unitOfWorkProvider)
        {
            this.postService = postService;
            this.voteService = voteService;
            this.imageService = imageService;
            this.postReplyService = postReplyService;
            this.hashtagService = hashtagService;
            this.friendshipService = friendshipService;
        }

        public async Task<PostDto> GetPostDtoAccordingToId(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await postService.GetAsync(id);
            }
        }

        public async Task<Guid> AddPost(UserDto user, PostCreateDto postDto)
        {
            PostDto post = null;
            if (postDto.HasAgeRestriction)
            {
                post = new PostDto
                {
                    Text = postDto.Text,
                    AgeRestrictionFrom = postDto.AgeRestrictionFrom,
                    AgeRestrictionTo = postDto.AgeRestrictionTo,
                    Time = DateTime.Now,
                    UserId = user.Id,
                    GenderRestriction = postDto.GenderRestriction,
                    HasAgeRestriction = postDto.HasAgeRestriction,
                    Visibility = postDto.Visibility
                };
            }
            else {
                post = new PostDto
                {
                    Text = postDto.Text,
                    AgeRestrictionFrom = 0,
                    AgeRestrictionTo = 0,
                    Time = DateTime.Now,
                    UserId = user.Id,
                    GenderRestriction = postDto.GenderRestriction,
                    HasAgeRestriction = postDto.HasAgeRestriction,
                    Visibility = postDto.Visibility
                };
            }

            var hashtags = FindHashtags(postDto.Text);
            using (var uow = UnitOfWorkProvider.Create())
            {
                var id = postService.Create(post);
                await uow.Commit();
                AddHashtagsToPost(postDto.Text, id);
                await uow.Commit();
                return id;
            }
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> GetPostFeedAsync(PostFilterDto filter, Guid userId, string hashtagFilter)
        {
            using (UnitOfWorkProvider.Create())
            {
                filter.SortCriteria = "Time";
                if (hashtagFilter != null)
                {
                    filter.PostIdsWithHashtag = (await hashtagService.ListHashtagAsync(new HashtagFilterDto { Tag = hashtagFilter }))
                        .Items
                        .Select(h => h.PostId)
                        .ToList();
                }
                if (userId != Guid.Empty)
                {
                    filter.LoggedUserId = userId;
                    return await postService.ListPostsAvailableForUser(userId, filter);
                }
                return await postService.ListPostAsync(filter);
            }
        }

        public async Task DeletePost(Guid postId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                postService.Delete(postId);
                await uow.Commit();
            }
        }
        
        public async Task<QueryResultDto<PostDto, PostFilterDto>> GetPostsByUserId(PostFilterDto filter, Guid userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                if (userId != Guid.Empty)
                {
                    filter.UserId = userId;
                    filter.SortCriteria = "Time";
                    return await postService.ListPostAsync(filter);
                }
                throw new ArgumentException("Cannot display posts of not existing user");
            }
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> GetPostsByUserIdForLoggedUser(PostFilterDto filter,
            Guid userId, Guid loggedUserId)
        {
            using (UnitOfWorkProvider.Create())
            {
                if (userId != Guid.Empty)
                {
                    var friendships = await friendshipService.GetFriendsOfUserAsync(loggedUserId, new FriendshipFilterDto());
                    if (friendships.Items.Any(friend => friend.Id == userId))
                    {
                        filter.PostUserIds = new List<Guid>
                        {
                            userId
                        };
                    }

                    filter.LoggedUserId = loggedUserId;
                    filter.UserId = userId;
                    filter.SortCriteria = "Time";
                    filter.IncludePrivatePosts = true;

                    return await postService.ListPostAsync(filter);
                }
                throw new ArgumentException("Cannot display posts of not existing user");
            }
        }

        public async Task<Tuple<bool, VoteDto>> VoteFromUser(Guid imageId, Guid userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var votes = await voteService.ListVoteAsync(new VoteFilterDto
                {
                    ImageId = imageId,
                    UserId = userId
                });
                if (votes.TotalItemsCount != 0)
                {
                    return new Tuple<bool, VoteDto>(true, votes.Items.First());
                }
                return new Tuple<bool, VoteDto>(false, null);
            }
        }

        public async Task<Guid> ChangeVote(Guid imageId, Guid userId, VoteType type)
        {
            var image = await GetImage(imageId); 
            var vote = await VoteFromUser(imageId, userId);
            if (vote.Item1)
            {
                // In case you disliked the image and you want to like it instead
                // (or other way round), remove the old vote and create new
                await RemoveVote(image, vote.Item2);
                if (vote.Item2.Type == type)
                {
                    return Guid.Empty;
                }
            }
            return await AddVote(image, userId, type);
        }

        private async Task<ImageDto> GetImage(Guid imageId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var image = await imageService.GetAsync(imageId);
                if (image == null)
                {
                    throw new ArgumentException("Image does not exist");
                }
                return image;
            }
        }

        private async Task<Guid> AddVote(ImageDto image, Guid userId, VoteType type)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (type == VoteType.Like)
                {
                    image.LikesCount++;
                }
                else
                {
                    image.DislikesCount++;
                }
                await imageService.Update(image);
                var guid = voteService.Create(new VoteDto { ImageId = image.Id, UserId = userId, Type = type });
                await uow.Commit();
                return guid;
            }
        }
        
        private async Task RemoveVote(ImageDto image, VoteDto vote)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (vote.Type == VoteType.Like)
                {
                    image.LikesCount--;
                }
                else
                {
                    image.DislikesCount--;
                }
                await imageService.Update(image);
                voteService.Delete(vote.Id);
                await uow.Commit();
            }
        }

        public async Task AddReplyToPost(Guid postId, Guid userId, string reply)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (reply.Length < MinimalPostReplyLength)
                {
                    throw new ArgumentException($"Comment must have atleast {MinimalPostReplyLength} characters");
                }
                var comment = new PostReplyDto
                {
                    PostId = postId,
                    UserId = userId,
                    Text = reply,
                    Time = DateTime.Now,
                };
                postReplyService.Create(comment);
                await uow.Commit();
            }
        }

        public async Task DeleteReply(Guid replyId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                postReplyService.Delete(replyId);
                await uow.Commit();
            }
        }

        public async Task<List<PostReplyDto>> ListOfReplysForPost(Guid postId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var result = await postReplyService.ListPostReplyAsync(new PostReplyFilterDto { PostId = postId });
                return result.Items.OrderBy(r => r.Time).ToList();
            }
        }

        public async Task<PostReplyDto> GetReplyById(Guid replyId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await postReplyService.GetAsync(replyId);
            }
        }

        public async Task<List<ImageDto>> ListOfImagesForPost(Guid postId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var result = await imageService.ListImageAsync(new ImageFilterDto { PostId = postId });
                return result.Items
                    .OrderByDescending(image => image.LikesCount - image.DislikesCount)
                    .ToList();
            }
        }

        public async Task<Guid> AddImage(ImageCreateDto imageCreateDto)
        {
            var image = new ImageDto
            {
                BinaryImage = imageCreateDto.BinaryImage,
                PostId = imageCreateDto.PostId,
                LikesCount = 0,
                DislikesCount = 0
            };

            using (var uow = UnitOfWorkProvider.Create())
            {
                var id = imageService.Create(image);
                await uow.Commit();
                return id;
            }
        }

        public static List<Tuple<int, int>> FindHashtagIndices(string text)
        {
            var indices = new List<Tuple<int, int>>();
            var index = text.IndexOf('#');
            while (index >= 0)
            {
                int tagEnd = index;
                while (tagEnd < text.Length && !char.IsWhiteSpace(text[tagEnd])) { tagEnd++; }
                indices.Add(new Tuple<int, int>(index, tagEnd));
                index = text.IndexOf('#', tagEnd);
            }
            return indices;
        }

        private static List<string> FindHashtags(string text)
        {
            return FindHashtagIndices(text)
                .Select(idx => text.Substring(idx.Item1, idx.Item2 - idx.Item1))
                .ToList();
        }

        private void AddHashtagsToPost(string text, Guid postId)
        {
            var hashtags = FindHashtags(text);
            foreach (var hashtag in hashtags)
            {
                hashtagService.Create(new HashtagDto { Tag = hashtag, PostId = postId });
            }
        }

        private async Task RemoveHashtagsForPost(Guid postId)
        {
            var oldHashtags = await hashtagService.ListHashtagAsync(new HashtagFilterDto { PostId = postId });
            foreach (var hashtag in oldHashtags.Items)
            {
                hashtagService.Delete(hashtag.Id);
            }
        }
    }
}
