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
    public class PostReplyQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_FilterByUsername_ReturnsCorrectSimplePredicate()
        {
            Guid filteredPostId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate = new SimplePredicate(nameof(PostReply.PostId), ValueComparingOperator.Equal, filteredPostId);  
            var mapperMock = mockManager.ConfigureMapperMock<PostReply, PostReplyDto, PostReplyFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<PostReply>();
            var postReplyQueryObject = new PostReplyQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new PostReplyFilterDto { PostId = filteredPostId };
            var temp = await postReplyQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_EmptyFilter_ReturnsNull()
        {
            var mockManager = new QueryMockManager();
            var mapperMock = mockManager.ConfigureMapperMock<PostReply, PostReplyDto, PostReplyFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<PostReply>();
            var postReplyQueryObject = new PostReplyQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new PostReplyFilterDto();
            var temp = await postReplyQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(null, mockManager.CapturedPredicate);
        }
    }
}