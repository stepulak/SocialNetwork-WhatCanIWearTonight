using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayerTesting.QueryObjectsTests.Common;
using EntityDatabase;
using NUnit.Framework;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;
using Gender = BusinessLayer.DataTransferObjects.Gender;
using PostVisibility = BusinessLayer.DataTransferObjects.PostVisibility;

namespace BusinessLayerTesting.QueryObjectsTests
{
    [TestFixture]
    public class PostQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_FilterByVisibility_ReturnsCorrectPredicate()
        {
            const PostVisibility filteredVisibility = PostVisibility.FriendsOnly;
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(Post.Visibility), ValueComparingOperator.Equal, filteredVisibility)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<Post, PostDto, PostFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Post>();
            var postQueryObject = new PostQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new PostFilterDto { Visibility = filteredVisibility };
            var temp = await postQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByUserId_ReturnsCorrectPredicate()
        {
            Guid filteredUserId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(Post.UserId), ValueComparingOperator.Equal, filteredUserId)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<Post, PostDto, PostFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Post>();
            var postQueryObject = new PostQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new PostFilterDto { UserId = filteredUserId };
            var temp = await postQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByGenderRestriction_ReturnsCorrectPredicate()
        {
            const Gender filteredGenderRestriction = Gender.Female;
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new CompositePredicate(
                        new List<IPredicate>
                        {
                            new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, filteredGenderRestriction),
                            new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, Gender.NoInformation)
                        }, LogicalOperator.OR)
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<Post, PostDto, PostFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Post>();
            var postQueryObject = new PostQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new PostFilterDto { GenderRestriction = filteredGenderRestriction };
            var temp = await postQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_FilterByUserAge_ReturnsCorrectPredicate()
        {
            const int filteredUserAge = 30;
            var mockManager = new QueryMockManager();
            var agePredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.HasAgeRestriction), ValueComparingOperator.Equal, false)
            };
            var innerAgePredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.AgeRestrictionTo), ValueComparingOperator.LessThanOrEqual, filteredUserAge),
                new SimplePredicate(nameof(Post.AgeRestrictionFrom), ValueComparingOperator.GreaterThanOrEqual, filteredUserAge)
            };
            agePredicates.Add(new CompositePredicate(innerAgePredicates, LogicalOperator.AND));
            
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new CompositePredicate(agePredicates, LogicalOperator.OR)
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<Post, PostDto, PostFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Post>();
            var postQueryObject = new PostQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new PostFilterDto { UserAge = filteredUserAge };
            var temp = await postQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        [Test]
        public async Task ApplyWhereClause_ComplexFilterByAllParameters_ReturnsCorrectPredicate()
        {
            const int filteredUserAge = 30;
            Gender filteredGenderRestriction = Gender.Female;
            PostVisibility filteredPostVisibility = PostVisibility.FriendsOnly;
            Guid filteredUserId = Guid.NewGuid();
            var mockManager = new QueryMockManager();
           
            var agePredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.HasAgeRestriction), ValueComparingOperator.Equal, false)
            };
            var innerAgePredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.AgeRestrictionTo), ValueComparingOperator.LessThanOrEqual, filteredUserAge),
                new SimplePredicate(nameof(Post.AgeRestrictionFrom), ValueComparingOperator.GreaterThanOrEqual, filteredUserAge)
            };
            agePredicates.Add(new CompositePredicate(innerAgePredicates, LogicalOperator.AND));

            var genderPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, filteredGenderRestriction),
                    new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, Gender.NoInformation)
                }, LogicalOperator.OR);
            
            var userIdPredicate = new SimplePredicate(nameof(Post.UserId), ValueComparingOperator.Equal, filteredUserId);

            var visibilityPredicate = new SimplePredicate(nameof(Post.Visibility), ValueComparingOperator.Equal, filteredPostVisibility);

            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new CompositePredicate(agePredicates, LogicalOperator.OR),
                    genderPredicate,
                    userIdPredicate,
                    visibilityPredicate
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<Post, PostDto, PostFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Post>();
            var postQueryObject = new PostQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new PostFilterDto
            {
                UserAge = filteredUserAge,
                GenderRestriction = filteredGenderRestriction,
                UserId = filteredUserId,
                Visibility = filteredPostVisibility
            };
            var temp = await postQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_EmptyFilter_ReturnsNull()
        {
            var mockManager = new QueryMockManager();
            var mapperMock = mockManager.ConfigureMapperMock<Post, PostDto, PostFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<Post>();
            var postQueryObject = new PostQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new PostFilterDto();
            var temp = await postQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(null, mockManager.CapturedPredicate);
        }
    }
    
}