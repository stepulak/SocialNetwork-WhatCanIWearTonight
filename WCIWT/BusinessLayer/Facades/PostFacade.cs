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

namespace BusinessLayer.Facades
{
    public class PostFacade : FacadeBase
    {
        public const int MinimalPostReplyLength = 2;

        private readonly IPostService postService;
        private readonly IVoteService voteService;
        private readonly IImageService imageService;
        private readonly IPostReplyService postReplyService;

        public PostFacade(IUnitOfWorkProvider unitOfWorkProvider, 
            IPostService postService, IPostReplyService postReplyService,
            IVoteService voteService, IImageService imageService)
            : base(unitOfWorkProvider)
        {
            this.postService = postService;
            this.voteService = voteService;
            this.imageService = imageService;
            this.postReplyService = postReplyService;
        }

        public async Task<PostDto> GetPostDtoAccordingToId(Guid id) => await postService.GetAsync(id);

        public Guid AddPost(UserDto user, PostDto post)
        {
            post.Time = DateTime.Now;
            post.UserId = user.Id;
            return postService.Create(post);
        }

        public async Task<QueryResultDto<PostDto, PostFilterDto>> GetPostFeedAsync(PostFilterDto filter, Guid userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                if (userId != Guid.Empty)
                {
                    return await postService.ListPostsAvailableForUser(userId, filter);
                }
                return await postService.ListPostAsync(filter);
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
                    filter.IncludePrivatePosts = true;
                    return await postService.ListPostAsync(filter);
                }
                throw new ArgumentException("Cannot display posts of not existing user");
            }
        }

        public void DeletePost(PostDto post) => postService.Delete(post.Id);
        
        public async Task<Tuple<bool, VoteDto>> VoteFromUser(Guid imageId, Guid userId)
        {
            var vote = await voteService.ListVoteAsync(new VoteFilterDto
            {
                ImageId = imageId,
                UserId = userId
            });
            if (vote.TotalItemsCount == 1)
            {
                return new Tuple<bool, VoteDto>(true, vote.Items.First());
            }
            return new Tuple<bool, VoteDto>(false, new VoteDto { });
        }

        public async Task<Guid> AddVote(Guid imageId, Guid userId, VoteType type)
        {
            var image = await imageService.GetAsync(imageId);
            if (image == null)
            {
                throw new ArgumentException("Image does not exist");
            }
            var vote = await VoteFromUser(imageId, userId);
            if (vote.Item1)
            {
                // In case you disliked the image and you want to like it instead
                // (or other way round), remove the old vote and create new
                await RemoveVote(vote.Item2);
            }
            if (type == VoteType.Like)
            {
                image.LikesCount++;
            }
            else
            {
                image.DislikesCount++;
            }
            await imageService.Update(image);
            return voteService.Create(new VoteDto { ImageId = imageId, UserId = userId, Type = type });
        }

        public async Task RemoveVote(Guid imageId, Guid userId)
        {
            var allVotes = await voteService.ListVoteAsync(new VoteFilterDto { ImageId = imageId, UserId = userId });
            await RemoveVote(allVotes.Items.First());
        }

        public async Task RemoveVote(VoteDto vote)
        {
            if (vote.Type == VoteType.Like)
            {
                vote.Image.LikesCount--;
            }
            else
            {
                vote.Image.DislikesCount--;
            }
            await imageService.Update(vote.Image);
            voteService.Delete(vote.Id);
        }

        public void CommentPost(Guid postId, Guid userId, string reply)
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
        }

        public async Task<List<PostReplyDto>> ListOfReplysForPost(Guid postId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var result = await postReplyService.ListPostReplyAsync(new PostReplyFilterDto { PostId = postId });
                return result.Items.OrderBy(r => r.Time).ToList();
            }
        }

        public async Task<List<ImageDto>> ListOfImagesForPost(Guid postId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var result = await imageService.ListImageAsync(new ImageFilterDto { PostId = postId });
                return result.Items.ToList();
            }
        }
    }
}
