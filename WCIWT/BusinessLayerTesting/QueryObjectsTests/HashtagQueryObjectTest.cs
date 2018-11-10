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
    public class HashtagQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_FilterByPostId_ReturnsCorrectPredicate()
        {
            Guid filteredPostId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(new List<IPredicate>
            {
                new SimplePredicate(nameof(Hashtag.PostId), ValueComparingOperator.Equal, filteredPostId)
            }, LogicalOperator.AND);
                
            var mapperMock = mockManager.ConfigureMapperMock<Hashtag, HashtagDto, HashtagFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Hashtag>();
            var hashtagQueryObject = new HashtagQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new HashtagFilterDto { PostId = filteredPostId };
            var temp = await hashtagQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByTagString_ReturnsCorrectPredicate()
        {
            const string filteredTag = "Hashtag";
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(new List<IPredicate>
            {
                new SimplePredicate(nameof(Hashtag.Tag), ValueComparingOperator.Equal, filteredTag)
            }, LogicalOperator.AND);
                
            var mapperMock = mockManager.ConfigureMapperMock<Hashtag, HashtagDto, HashtagFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Hashtag>();
            var hashtagQueryObject = new HashtagQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new HashtagFilterDto { Tag = filteredTag };
            var temp = await hashtagQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_ComplexFilterByPostIdAndTagString_ReturnsCorrectPredicate()
        {
            Guid filteredPostId = Guid.NewGuid();
            const string filteredTag = "Hashtag";
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(new List<IPredicate>
            {
                new SimplePredicate(nameof(Hashtag.Tag), ValueComparingOperator.Equal, filteredTag),
                new SimplePredicate(nameof(Hashtag.PostId), ValueComparingOperator.Equal, filteredPostId)
            }, LogicalOperator.AND);
                
            var mapperMock = mockManager.ConfigureMapperMock<Hashtag, HashtagDto, HashtagFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Hashtag>();
            var hashtagQueryObject = new HashtagQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new HashtagFilterDto { PostId = filteredPostId, Tag = filteredTag };
            var temp = await hashtagQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_EmptyFilter_ReturnsNull()
        {
            var mockManager = new QueryMockManager();
            var mapperMock = mockManager.ConfigureMapperMock<Hashtag, HashtagDto, HashtagFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Hashtag>();
            var hashtagQueryObject = new HashtagQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new HashtagFilterDto();
            var temp = await hashtagQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(null, mockManager.CapturedPredicate);
        }
    }
}