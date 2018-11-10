using AutoMapper;
using BusinessLayer.DataTransferObjects.Common;
using Moq;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;
using WCIWT.Infrastructure.Query.Predicates;

namespace BusinessLayerTesting.QueryObjectsTests.Common
{
    internal class QueryMockManager
    {
        internal IPredicate CapturedPredicate { get; private set; }
        
        internal Mock<IQuery<TEntity>> ConfigureQueryMock<TEntity>() where TEntity : class, IEntity, new()
        {
            var queryMock = new Mock<IQuery<TEntity>>(MockBehavior.Loose);
            queryMock
                .Setup(productQuery => productQuery.Where(It.IsAny<IPredicate>()))
                .Callback<IPredicate>(param => CapturedPredicate = param)
                .Returns(() => queryMock.Object);
            return queryMock;
        }

        internal Mock<IMapper> ConfigureMapperMock<TEntity, TDto, TFilterDto>() where TFilterDto : FilterDtoBase where TEntity : IEntity
        {
            var mapperMock = new Mock<IMapper>(MockBehavior.Loose);
            mapperMock
                .Setup(mapper => mapper.Map<QueryResultDto<TDto, TFilterDto>>(It.IsAny<QueryResult<TEntity>>()))
                .Returns(() => new QueryResultDto<TDto, TFilterDto>());
            return mapperMock;
        }
    }
}