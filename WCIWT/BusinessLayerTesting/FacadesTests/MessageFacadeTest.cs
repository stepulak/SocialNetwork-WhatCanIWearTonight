using BusinessLayer.DataTransferObjects;
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
using BusinessLayer.Services.Messages;
using BusinessLayer.Services.Users;

namespace BusinessLayerTesting.FacadesTests
{
    [TestFixture]
    public class MessageFacadeTest
    {
        public UserService CreateUserService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<UserDto, User, UserFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<User>().Object;
            return new UserService(mapperMock, repositoryMock, queryMock);
        }

        public MessageService CreateMessagesService(FacadeMockManager mockManager)
        {
            var mapperMock = mockManager.ConfigureMapperMock<Message, MessageDto, MessageFilterDto>();
            var queryMock = mockManager.ConfigureQueryObjectMock<MessageDto, Message, MessageFilterDto>(null).Object;
            var repositoryMock = mockManager.ConfigureRepositoryMock<Message>().Object;
            return new MessageService(mapperMock, repositoryMock, queryMock);
        }

        public Tuple<Guid, Guid> CreateSampleUsers(UserService service)
        {
            var id1 = service.Create(new UserDto
            {
                Email = "missfortune@lol.com",
                Username = "Miss Fortune",
            });
            var id2 = service.Create(new UserDto
            {
                Email = "sivir@lol.com",
                Username = "Sivir",
            });
            return new Tuple<Guid, Guid>(id1, id2);
        }

        [Test]
        public async Task SendMessage_Success()
        {
            var mockManager = new FacadeMockManager();
            var unitOfWorkMock = FacadeMockManager.ConfigureUowMock().Object;
            var userService = CreateUserService(mockManager);
            var messageService = CreateMessagesService(mockManager);
            var facade = new MessageFacade(unitOfWorkMock, messageService, userService);
            var ids = CreateSampleUsers(userService);
            var msgText = "Hello World";
            var guid = facade.SendMessage(new UserDto { Id = ids.Item1 }, new UserDto { Id = ids.Item2 }, msgText);
            var dto = await messageService.GetAsync(guid);
            Assert.IsFalse(dto.Seen);
            Assert.AreEqual(dto.UserSenderId, ids.Item1);
            Assert.AreEqual(dto.UserReceiverId, ids.Item2);
            Assert.AreEqual(dto.Text, msgText);
        }

        [Test]
        public async Task ReceivedMessages_Success()
        {
            var mockManager = new FacadeMockManager();
            var unitOfWorkMock = FacadeMockManager.ConfigureUowMock().Object;
            var userService = CreateUserService(mockManager);
            var messageService = CreateMessagesService(mockManager);
            var facade = new MessageFacade(unitOfWorkMock, messageService, userService);
            var ids = CreateSampleUsers(userService);
            var numMessages = 10;
            var msgText = "Hello World";
            for (int i = 0; i < numMessages; i++)
            {
                facade.SendMessage(new UserDto { Id = ids.Item1 }, new UserDto { Id = ids.Item2 }, msgText);
            }
            var msgs = await facade.ReceivedMessages(new UserDto { Id = ids.Item1 });
            Assert.AreEqual(msgs.Count, 10);
            msgs.ForEach(m =>
            {
                Assert.AreEqual(m.UserSenderId, ids.Item1);
                Assert.AreEqual(m.UserReceiverId, ids.Item2);
                Assert.AreEqual(m.Text, msgText);
            });
        }
    }
}
