using BusinessLayer.DataTransferObjects;
using BusinessLayer.Facades.Common;
using BusinessLayer.Services.PostServices;
using BusinessLayer.Services.UserServices;
using BusinessLayer.DataTransferObjects.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects.Common;
using WCIWT.Infrastructure.UnitOfWork;

namespace BusinessLayer.Facades
{
    public class PostFacade : FacadeBase
    {
        public const int MinimalPostReplyLength = 2;

        private readonly PostService postService;
        private readonly VoteService voteService;
        private readonly ImageService imageService;
        private readonly PostReplyService postReplyService;

        public PostFacade(IUnitOfWorkProvider unitOfWorkProvider, 
            PostService postService, PostReplyService postReplyService,
            VoteService voteService, ImageService imageService)
            : base(unitOfWorkProvider)
        {
            this.postService = postService;
            this.voteService = voteService;
            this.imageService = imageService;
            this.postReplyService = postReplyService;
        }

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

        public void DeletePost(PostDto post) => postService.Delete(post.Id);
        
        public async Task<Tuple<bool, VoteDto>> VoteFromUser(ImageDto image, UserDto user)
        {
            var vote = await voteService.ListVoteAsync(new VoteFilterDto
            {
                ImageId = image.Id,
                UserId = user.Id
            });
            if (vote.TotalItemsCount == 1)
            {
                return new Tuple<bool, VoteDto>(true, vote.Items.First());
            }
            return new Tuple<bool, VoteDto>(true, new VoteDto { });
        }

        public async Task<Guid> AddVote(ImageDto image, UserDto user, VoteType type)
        {
            var vote = await VoteFromUser(image, user);
            if (vote.Item1 == true)
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
            return voteService.Create(new VoteDto { ImageId = image.Id, UserId = user.Id, Type = type });
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
    }
}
