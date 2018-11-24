using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using EntityDatabase;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;

namespace BusinessLayerTesting.QueryObjectsTests.Common
{
    [TestFixture]
    public class MessageQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_FilterByUserId_ReturnsCorrectPredicate()
        {
            Guid filteredUserId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(Message.UserReceiverId), ValueComparingOperator.Equal, filteredUserId),
                    new SimplePredicate(nameof(Message.UserSenderId), ValueComparingOperator.Equal, filteredUserId) 
                }, LogicalOperator.OR);
            var mapperMock = mockManager.ConfigureMapperMock<Message, MessageDto, MessageFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Message>();
            var messageQueryObject = new MessageQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new MessageFilterDto { UserId = filteredUserId };
            var temp = await messageQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByUserIdAndTypeReceiver_ReturnsCorrectPredicate()
        {
            Guid filteredUserId = Guid.NewGuid();
            MessageUserFilterType filterType = MessageUserFilterType.Receiver;
            
            var mockManager = new QueryMockManager();
            var expectedPredicate = new SimplePredicate(nameof(Message.UserReceiverId), ValueComparingOperator.Equal,
                filteredUserId);
            var mapperMock = mockManager.ConfigureMapperMock<Message, MessageDto, MessageFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Message>();
            var messageQueryObject = new MessageQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new MessageFilterDto { UserId = filteredUserId, UserFilterType = filterType};
            var temp = await messageQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByUserIdAndTypeSender_ReturnsCorrectPredicate()
        {
            Guid filteredUserId = Guid.NewGuid();
            MessageUserFilterType filterType = MessageUserFilterType.Sender;
            
            var mockManager = new QueryMockManager();
            var expectedPredicate = new SimplePredicate(nameof(Message.UserSenderId), ValueComparingOperator.Equal,
                filteredUserId);
            var mapperMock = mockManager.ConfigureMapperMock<Message, MessageDto, MessageFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Message>();
            var messageQueryObject = new MessageQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new MessageFilterDto { UserId = filteredUserId, UserFilterType = filterType};
            var temp = await messageQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_EmptyFilter_ReturnsNull()
        {
            var mockManager = new QueryMockManager();
            var mapperMock = mockManager.ConfigureMapperMock<Message, MessageDto, MessageFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Message>();
            var messageQueryObject = new MessageQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new MessageFilterDto();
            var temp = await messageQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(null, mockManager.CapturedPredicate);
        }
    }
}