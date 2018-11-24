using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Facades;
using BusinessLayerTesting.FacadesTests.Common;
using EntityDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Services.Friendships;
using BusinessLayer.Services.Users;
using WCIWT.Infrastructure;

namespace BusinessLayerTesting.FacadesTests
{
    [TestFixture]
    class UserFacadeTest
    {
        public UserService CreateUserService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<UserDto, User, UserFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<User>().Object;
            return new UserService(mapperMock, repositoryMock, null, queryMock);
        }

        public FriendshipService CreateFriendshipService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<Friendship, FriendshipDto, FriendshipFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<FriendshipDto, Friendship, FriendshipFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<Friendship>().Object;
            return new FriendshipService(mapperMock, repositoryMock, null, queryMock);
        }

        [Test]
        public async Task RegisterUser_Success()
        {
            var mockManager = new FacadeMockManager();
            var unitOfWorkMock = FacadeMockManager.ConfigureUowMock().Object;
            var userService = CreateUserService(mockManager);
            var friendshipService = CreateFriendshipService(mockManager);
            var facade = new UserFacade(unitOfWorkMock, userService, friendshipService);
            var guid = await facade.RegisterUser(new UserDto
            {
                Email = "missfortune@lol.com",
                Username = "Miss Fortune",
                PasswordHash = "0xBEEF"
            });
            var expected = await userService.GetAsync(guid);
            Assert.AreEqual(expected.Id, guid);
        }

        [Test]
        public async Task UnregisterUser_Success()
        {
            var mockManager = new FacadeMockManager();
            var unitOfWorkMock = FacadeMockManager.ConfigureUowMock().Object;
            var userService = CreateUserService(mockManager);
            var friendshipService = CreateFriendshipService(mockManager);
            var facade = new UserFacade(unitOfWorkMock, userService, friendshipService);
            var dto = new UserDto
            {
                Email = "missfortune@lol.com",
                Username = "Miss Fortune",
                PasswordHash = "0xBEEF"
            };
            var guid = await facade.RegisterUser(dto);
            await facade.UnregisterUser(new UserDto { Id = guid });
            Assert.AreEqual((await facade.GetAllUsersAsync(new UserFilterDto { Email = "missfortune@lol.com" })).TotalItemsCount, 0);
        }
    }
}
