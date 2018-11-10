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
        private const string LambdaParameterName = "param";

        private readonly ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), LambdaParameterName);

        protected DbContext Context => ((EntityFrameworkUnitOfWork)Provider.GetUnitOfWorkInstance()).Context;

        public EntityFrameworkQuery(IUnitOfWorkProvider provider) : base(provider) { }

        public override async Task<QueryResult<TEntity>> ExecuteAsync()
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();

            if (string.IsNullOrWhiteSpace(SortAccordingTo) && SelectedPage.HasValue)
            {
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
            if (SelectedPage.HasValue)
            {
                queryable = queryable.Skip(PageSize * (SelectedPage.Value - 1)).Take(PageSize);
            }
            var items = await queryable.ToListAsync();
            return new QueryResult<TEntity>(items, itemsCount, PageSize, SelectedPage);
        }

        private IQueryable<TEntity> UseFilterCriteria(IQueryable<TEntity> queryable)
        {
            var property = typeof(TEntity).GetProperty(SortAccordingTo);
            var parameter = Expression.Parameter(typeof(TEntity), "i");
            var expression = Expression.Lambda(Expression.Property(parameter, property), parameter);
            return (IQueryable<TEntity>)typeof(EntityFrameworkQuery<TEntity>)
                .GetMethod(nameof(UseSortCriteriaCore), BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(property.PropertyType)
                .Invoke(this, new object[] { expression, queryable });
        }

        private IQueryable<TEntity> UseSortCriteriaCore<TKey>(Expression<Func<TEntity, TKey>> sortExpression, IQueryable<TEntity> queryable)
        {
            return UseAscendingOrder ? queryable.OrderBy(sortExpression) : queryable.OrderByDescending(sortExpression);
        }

        private IQueryable<TEntity> UseSortCriteria(IQueryable<TEntity> queryable)
        {
            var bodyExpression = Predicate is CompositePredicate composite
                ? CombineBinaryExpressions(composite)
                : BuildBinaryExpression(Predicate as SimplePredicate);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(bodyExpression, parameterExpression);
            return queryable.Where(lambdaExpression);
        }

        private Expression BuildBinaryExpression(IPredicate predicate)
        {
            var simplePredicate = predicate as SimplePredicate;
            if (simplePredicate == null)
            {
                throw new ArgumentException("Expected predicate to be of type SimplePredicate");
            }
            return simplePredicate.GetExpression(parameterExpression);
        }

        private Expression CombineBinaryExpressions(CompositePredicate compositePredicate)
        {
            if (compositePredicate.Predicates.Count == 0)
            {
                throw new InvalidOperationException("At least one simple predicate mus be given");
            }
            var expression = compositePredicate.Predicates.First() is CompositePredicate composite
                ? CombineBinaryExpressions(composite)
                : BuildBinaryExpression(compositePredicate.Predicates.First());

            for (var i = 1; i < compositePredicate.Predicates.Count; i++)
            {
                if (compositePredicate.Predicates[i] is CompositePredicate predicate)
                {
                    expression = compositePredicate.Operator == LogicalOperator.OR
                        ? Expression.OrElse(expression, CombineBinaryExpressions(predicate))
                        : Expression.AndAlso(expression, CombineBinaryExpressions(predicate));
                }
                else
                {
                    expression = compositePredicate.Operator == LogicalOperator.OR
                        ? Expression.OrElse(expression, BuildBinaryExpression(compositePredicate.Predicates[i]))
                        : Expression.AndAlso(expression, BuildBinaryExpression(compositePredicate.Predicates[i]));
                }
            }
            return expression;
        }
    }
}
