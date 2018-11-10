using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayerTesting.QueryObjectsTests.Common;
using EntityDatabase;
using NUnit.Framework;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;
using Gender = BusinessLayer.DataTransferObjects.Gender;

namespace BusinessLayerTesting.QueryObjectsTests
{
    [TestFixture]
    public class UserQueryObjectTest
    {
        [Test]
        public async Task ApplyWhereClause_SimpleFilterByUsername_ReturnsCorrectPredicate()
        {
            const string filteredUsername = "TestUser";
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                  new SimplePredicate(nameof(User.Username), ValueComparingOperator.Equal, filteredUsername)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<User>();
            var userQueryObject = new UserQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new UserFilterDto { Username = filteredUsername };
            var temp = await userQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_SimpleFilterByGender_ReturnsCorrectPredicate()
        {
            const Gender filteredGender = Gender.Male;
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(User.Gender), ValueComparingOperator.Equal, filteredGender)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<User>();
            var userQueryObject = new UserQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new UserFilterDto { Gender = filteredGender };
            var temp = await userQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_SimpleFilterByEmail_ReturnsCorrectPredicate()
        {
            const string filteredEmail = "test@user.com";
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(User.Email), ValueComparingOperator.Equal, filteredEmail)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<User>();
            var userQueryObject = new UserQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new UserFilterDto { Email = filteredEmail };
            var temp = await userQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_SimpleFilterByBornBefore_ReturnsCorrectPredicate()
        {
            DateTime filterBornBefore = new DateTime(2018,12,12);
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(User.Birthdate), ValueComparingOperator.LessThan, filterBornBefore)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<User>();
            var userQueryObject = new UserQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new UserFilterDto { BornBefore = filterBornBefore };
            var temp = await userQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_SimpleFilterByBornAfter_ReturnsCorrectPredicate()
        {
            DateTime filterBornAfter = new DateTime(2018,12,12);
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(User.Birthdate), ValueComparingOperator.GreaterThan, filterBornAfter)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<User>();
            var userQueryObject = new UserQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new UserFilterDto { BornAfter = filterBornAfter };
            var temp = await userQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_ComplexFilterUsernameAndEmail_ReturnsCorrectCompositePredicate()
        {
            const string filteredUsername = "TestUser";
            const string filteredEmail = "test@user.com";
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(User.Username), ValueComparingOperator.Equal, filteredUsername),
                    new SimplePredicate(nameof(User.Email), ValueComparingOperator.Equal, filteredEmail)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<User>();
            var userQueryObject = new UserQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new UserFilterDto { Username = filteredUsername, Email = filteredEmail };
            var temp = await userQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_ComplexFilterBornBeforeAndAfter_ReturnsCorrectCompositePredicate()
        {
            DateTime filterBornBefore = new DateTime(2018,12,12);
            DateTime filterBornAfter = new DateTime(2018,12,12);
            var mockManager = new QueryMockManager();
            var expectedPredicate = new CompositePredicate(
                new List<IPredicate>
                {
                    new SimplePredicate(nameof(User.Birthdate), ValueComparingOperator.LessThan, filterBornBefore),
                    new SimplePredicate(nameof(User.Birthdate), ValueComparingOperator.GreaterThan, filterBornAfter)  
                }, LogicalOperator.AND);
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<User>();
            var userQueryObject = new UserQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new UserFilterDto { BornBefore = filterBornBefore, BornAfter = filterBornAfter };
            var temp = await userQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(expectedPredicate, mockManager.CapturedPredicate);
        }
        
        [Test]
        public async Task ApplyWhereClause_EmptyFilter_ReturnsNull()
        {
            var mockManager = new QueryMockManager();
            var mapperMock = mockManager.ConfigureMapperMock<User, UserDto, UserFilterDto>();
            var queryMock = mockManager.ConfigureQueryMock<User>();
            var userQueryObject = new UserQueryObject(mapperMock.Object, queryMock.Object);

            var filter = new UserFilterDto();
            var temp = await userQueryObject.ExecuteQuery(filter);
            Assert.AreEqual(null, mockManager.CapturedPredicate);
        }
    }
}