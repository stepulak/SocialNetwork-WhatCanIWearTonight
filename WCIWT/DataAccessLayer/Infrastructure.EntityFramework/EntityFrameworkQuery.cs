using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.EntityFramework.UnitOfWork;
using WCIWT.Infrastructure.Query;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;
using WCIWT.Infrastructure.UnitOfWork;

namespace WCIWT.Infrastructure.EntityFramework
{
    public class EntityFrameworkQuery<TEntity> : QueryBase<TEntity> where TEntity : class, IEntity, new()
    {
        private const string LamdaParameterName = "param";

        private readonly ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), LamdaParameterName);

        protected DbContext Context => ((EntityFrameworkUnitOfWork)Provider.GetUnitOfWorkInstance()).Context;

        /// <summary>
        ///   Initializes a new instance of the <see cref="EntityFrameworkQuery{TResult}" /> class.
        /// </summary>
        public EntityFrameworkQuery(IUnitOfWorkProvider provider) : base(provider) { }

        public override async Task<QueryResult<TEntity>> ExecuteAsync()
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();

            if (string.IsNullOrWhiteSpace(SortAccordingTo) && DesiredPage.HasValue)
            {
                // Sorting must always take place when paging is required
                SortAccordingTo = nameof(IEntity.Id);
                UseAscendingOrder = true;
            }
            if (SortAccordingTo != null)
            {
                queryable = UseSortCriteria(queryable);
            }
            if (Predicate != null)
            {
                queryable = UseFilterCriteria(queryable);
            }
            var itemsCount = queryable.Count();
            if (DesiredPage.HasValue)
            {
                queryable = queryable.Skip(PageSize * (DesiredPage.Value - 1)).Take(PageSize);
            }
            var items = await queryable.ToListAsync();
            return new QueryResult<TEntity>(items, itemsCount, PageSize, DesiredPage);
        }

        private IQueryable<TEntity> UseSortCriteria(IQueryable<TEntity> queryable)
        {
            var prop = typeof(TEntity).GetProperty(SortAccordingTo);
            var param = Expression.Parameter(typeof(TEntity), "i");
            var expr = Expression.Lambda(Expression.Property(param, prop), param);
            return (IQueryable<TEntity>)typeof(EntityFrameworkQuery<TEntity>)
                .GetMethod(nameof(UseSortCriteriaCore), BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(prop.PropertyType)
                .Invoke(this, new object[] { expr, queryable });
        }

        private IQueryable<TEntity> UseSortCriteriaCore<TKey>(Expression<Func<TEntity, TKey>> sortExpression,
            IQueryable<TEntity> queryable)
        {
            return UseAscendingOrder ? queryable.OrderBy(sortExpression) : queryable.OrderByDescending(sortExpression);
        }

        private IQueryable<TEntity> UseFilterCriteria(IQueryable<TEntity> queryable)
        {
            var bodyExpression = Predicate is CompositePredicate composite ? CombineBinaryExpressions(composite) : BuildBinaryExpression(Predicate as SimplePredicate);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(bodyExpression, parameterExpression);
            //Debug.WriteLine(lambdaExpression.ToString());
            return queryable.Where(lambdaExpression);
        }
        
        private Expression CombineBinaryExpressions(CompositePredicate compositePredicate)
        {
            if (compositePredicate.Predicates.Count == 0)
            {
                throw new InvalidOperationException("At least one simple predicate must be given");
            }
            var expression = compositePredicate.Predicates.First() is CompositePredicate composite
                ? CombineBinaryExpressions(composite)
                : BuildBinaryExpression(compositePredicate.Predicates.First());
            for (var i = 1; i < compositePredicate.Predicates.Count; i++)
            {
                if (compositePredicate.Predicates[i] is CompositePredicate predicate)
                {
                    expression = compositePredicate.Operator == LogicalOperator.OR ?
                        Expression.OrElse(expression, CombineBinaryExpressions(predicate)) :
                        Expression.AndAlso(expression, CombineBinaryExpressions(predicate));
                }
                else
                {
                    expression = compositePredicate.Operator == LogicalOperator.OR ?
                        Expression.OrElse(expression, BuildBinaryExpression(compositePredicate.Predicates[i])) :
                        Expression.AndAlso(expression, BuildBinaryExpression(compositePredicate.Predicates[i]));
                }
            }
            return expression;
        }

        private Expression BuildBinaryExpression(IPredicate predicate)
        {
            var simplePredicate = predicate as SimplePredicate;
            if (simplePredicate == null)
            {
                throw new ArgumentException("Expected simple predicate!");
            }
            return simplePredicate.GetExpression(parameterExpression);
        }
    }
}
