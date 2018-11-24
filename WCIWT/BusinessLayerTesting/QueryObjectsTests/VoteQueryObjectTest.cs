using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayerTesting.QueryObjectsTests.Common;
using EntityDatabase;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;

namespace BusinessLayerTesting.QueryObjectsTests
{
    [TestFixture]
    public class VoteQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_FilterByUserId_ReturnsCorrectSimplePredicate()
        {
            Guid filterUserId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate = new SimplePredicate(nameof(Vote.UserId), ValueComparingOperator.Equal, filterUserId);
            var mapperMock = mockManager.ConfigureMapperMock<Vote, VoteDto, VoteFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Vote>();
            var voteQueryObject = new VoteQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new VoteFilterDto { UserId = filterUserId };
            var temp = await voteQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByImageId_ReturnsCorrectSimplePredicate()
        {
            Guid filterImageId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate = new SimplePredicate(nameof(Vote.ImageId), ValueComparingOperator.Equal, filterImageId);
            var mapperMock = mockManager.ConfigureMapperMock<Vote, VoteDto, VoteFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Vote>();
            var voteQueryObject = new VoteQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new VoteFilterDto { ImageId = filterImageId };
            var temp = await voteQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_ComplexFilterByUserIdAndImageId_ReturnsCorrectCompositePredicate()
        {
            Guid filterImageId = Guid.NewGuid();
            Guid filterUserId = Guid.NewGuid();
            
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(Vote.ImageId), ValueComparingOperator.Equal, filterImageId),
                    new SimplePredicate(nameof(Vote.UserId), ValueComparingOperator.Equal, filterUserId)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<Vote, VoteDto, VoteFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Vote>();
            var voteQueryObject = new VoteQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new VoteFilterDto { UserId = filterUserId, ImageId = filterImageId };
            var temp = await voteQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_EmptyFilter_ReturnsNull()
        {
            var mockManager = new QueryMockManager();
            var mapperMock = mockManager.ConfigureMapperMock<Vote, VoteDto, VoteFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Vote>();
            var voteQueryObject = new VoteQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new VoteFilterDto();
            var temp = await voteQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(null, mockManager.CapturedPredicate);
        }
    }
}