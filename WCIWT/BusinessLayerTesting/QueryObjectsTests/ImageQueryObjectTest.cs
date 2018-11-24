using System;
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
    public class ImageQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_FilterByUserId_ReturnsCorrectPredicate()
        {
            Guid filteredPostId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate =
                new SimplePredicate(nameof(Image.PostId), ValueComparingOperator.Equal, filteredPostId);
            var mapperMock = mockManager.ConfigureMapperMock<Image, ImageDto, ImageFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Image>();
            var imageQueryObject = new ImageQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new ImageFilterDto { PostId = filteredPostId };
            var temp = await imageQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_EmptyFilter_ReturnsNull()
        {
            var mockManager = new QueryMockManager();

            var mapperMock = mockManager.ConfigureMapperMock<Image, ImageDto, ImageFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Image>();
            var imageQueryObject = new ImageQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new ImageFilterDto();
            var temp = await imageQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(null, mockManager.CapturedPredicate);
        }
    }
}