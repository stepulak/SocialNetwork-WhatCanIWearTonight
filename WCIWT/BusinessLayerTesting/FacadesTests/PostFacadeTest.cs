using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Facades;
using BusinessLayer.Services.PostServices;
using BusinessLayer.Services.UserServices;
using BusinessLayerTesting.FacadesTests.Common;
using EntityDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostVisibility = BusinessLayer.DataTransferObjects.PostVisibility;

namespace BusinessLayerTesting.FacadesTests
{
    [TestFixture]
    class PostFacadeTest
    {
        public UserService CreateUserService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<UserDto, User, UserFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<User>().Object;
            return new UserService(mapperMock, repositoryMock, null, queryMock);
        }

        public HashtagService CreateHashtagService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<Hashtag, HashtagDto, HashtagFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<HashtagDto, Hashtag, HashtagFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<Hashtag>().Object;
            return new HashtagService(mapperMock, repositoryMock, null, queryMock);
        }

        public PostService CreatePostService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<Post, PostDto, PostFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<PostDto, Post, PostFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<Post>().Object;
            return new PostService(mapperMock, repositoryMock, null, queryMock, CreateHashtagService(mockManager));
        }

        public PostReplyService CreatePostReplyService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<PostReply, PostReplyDto, PostReplyFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<PostReplyDto, PostReply, PostReplyFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<PostReply>().Object;
            return new PostReplyService(mapperMock, repositoryMock, null, queryMock);
        }

        public VoteService CreateVoteService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<Vote, VoteDto, VoteFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<VoteDto, Vote, VoteFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<Vote>().Object;
            return new VoteService(mapperMock, repositoryMock, null, queryMock);
        }

        public ImageService CreateImageService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<Image, ImageDto, ImageFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<ImageDto, Image, ImageFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<Image>().Object;
            return new ImageService(mapperMock, repositoryMock, null, queryMock);
        }

        public Guid CreateSampleUser(UserService service)
        {
            return service.Create(new UserDto
            {
                Email = "missfortune@lol.com",
                Username = "Miss Fortune",
            });
        }

        [Test]
        public async Task AddPost()
        {
            var mockManager = new FacadeMockManager();
            var unitOfWorkMock = FacadeMockManager.ConfigureUowMock().Object;
            var postService = CreatePostService(mockManager);
            var postReplyService = CreatePostReplyService(mockManager);
            var voteService = CreateVoteService(mockManager);
            var imageService = CreateImageService(mockManager);
            var userService = CreateUserService(mockManager);
            var facade = new PostFacade(unitOfWorkMock, postService, postReplyService, voteService, imageService);
            var userId = CreateSampleUser(userService);
            var text = "Is this sexy enough?";
            var postDto = new PostDto
            {
                HasAgeRestriction = false,
                Visibility = PostVisibility.Public,
                Text = text,
            };
            var guid = facade.AddPost(new UserDto { Id = userId }, postDto);
            var post = await postService.GetAsync(guid);
            Assert.AreEqual(post.Text, text);
            Assert.AreEqual(post.UserId, userId);
            Assert.IsFalse(post.HasAgeRestriction);
            Assert.AreEqual(post.Visibility, PostVisibility.Public);
        }
    }
}
