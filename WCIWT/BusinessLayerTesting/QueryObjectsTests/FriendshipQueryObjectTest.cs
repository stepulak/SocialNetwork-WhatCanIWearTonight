using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayerTesting.QueryObjectsTests.Common;
using EntityDatabase;
using NUnit.Framework;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;

namespace BusinessLayerTesting.QueryObjectsTests
{
    [TestFixture]
    public class FriendshipQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_FilterByUserAId_ReturnsCorrectPredicate()
        {
            Guid filteredUserId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.ApplicantId), ValueComparingOperator.Equal, filteredUserId),
                new SimplePredicate(nameof(Friendship.RecipientId), ValueComparingOperator.Equal, filteredUserId)
            }, LogicalOperator.OR);
            var mapperMock = mockManager.ConfigureMapperMock<Friendship, FriendshipDto, FriendshipFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Friendship>();
            var friendshipQueryObject = new FriendshipQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new FriendshipFilterDto { UserA = filteredUserId };
            var temp = await friendshipQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByUserBId_ReturnsCorrectPredicate()
        {
            Guid filteredUserId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.ApplicantId), ValueComparingOperator.Equal, filteredUserId),
                new SimplePredicate(nameof(Friendship.RecipientId), ValueComparingOperator.Equal, filteredUserId)
            }, LogicalOperator.OR);
            var mapperMock = mockManager.ConfigureMapperMock<Friendship, FriendshipDto, FriendshipFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Friendship>();
            var friendshipQueryObject = new FriendshipQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new FriendshipFilterDto { UserB = filteredUserId };
            var temp = await friendshipQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByBothUserIds_ReturnsCorrectPredicate()
        {
            Guid filteredUserAId = Guid.NewGuid();
            Guid filteredUserBId = Guid.NewGuid();

            var mockManager = new QueryMockManager();
            
            var userAIsApplicantPredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.ApplicantId), ValueComparingOperator.Equal, filteredUserAId),
                new SimplePredicate(nameof(Friendship.RecipientId), ValueComparingOperator.Equal, filteredUserBId)
            };
            
            var userBIsApplicantPredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.ApplicantId), ValueComparingOperator.Equal, filteredUserBId),
                new SimplePredicate(nameof(Friendship.RecipientId), ValueComparingOperator.Equal, filteredUserAId)
            };
            
            var compositeOfUserAIsApplicant = new CompositePredicate(userAIsApplicantPredicates, LogicalOperator.AND);
            var compositeOfUserBIsApplicant = new CompositePredicate(userBIsApplicantPredicates, LogicalOperator.AND);
            var expectedPredicate = new CompositePredicate(new List<IPredicate>
            {
                compositeOfUserAIsApplicant,
                compositeOfUserBIsApplicant
            }, LogicalOperator.OR);
            var mapperMock = mockManager.ConfigureMapperMock<Friendship, FriendshipDto, FriendshipFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Friendship>();
            var friendshipQueryObject = new FriendshipQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new FriendshipFilterDto { UserA = filteredUserAId, UserB = filteredUserBId};
            var temp = await friendshipQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_EmptyFilter_ReturnsNull()
        {
            var mockManager = new QueryMockManager();
            var mapperMock = mockManager.ConfigureMapperMock<Friendship, FriendshipDto, FriendshipFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Friendship>();
            var friendshipQueryObject = new FriendshipQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new FriendshipFilterDto();
            var temp = await friendshipQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(null, mockManager.CapturedPredicate);
        }
    }
}