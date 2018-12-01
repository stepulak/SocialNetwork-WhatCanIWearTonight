using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;

namespace WCIWT.Infrastructure.EntityFramework
{
    public static class SimplePredicateExtension
    {
        private static readonly IDictionary<ValueComparingOperator, Func<MemberExpression, ConstantExpression, Expression>> Expressions =
            new Dictionary<ValueComparingOperator, Func<MemberExpression, ConstantExpression, Expression>>
        {
            {ValueComparingOperator.Equal, Expression.Equal },
            {ValueComparingOperator.NotEqual, Expression.NotEqual },
            {ValueComparingOperator.GreaterThan, Expression.GreaterThan },
            {ValueComparingOperator.GreaterThanOrEqual, Expression.GreaterThanOrEqual },
            {ValueComparingOperator.LessThan, Expression.LessThan },
            {ValueComparingOperator.LessThanOrEqual, Expression.LessThanOrEqual },
            {ValueComparingOperator.StringContains, (memberExpression, constantExpression) =>
                Expression.Call(memberExpression, typeof(string).GetMethod("Contains", new[] { typeof(string) }), constantExpression)}
        };

        public static Expression GetExpression(this SimplePredicate simplePredicate, ParameterExpression parameterExpression)
        {
            var memberExpression = Expression.PropertyOrField(parameterExpression, simplePredicate.TargetPropertyName);
            // Ensure compared value has the same type as the accessed member 
            var memberType = GetMemberType(simplePredicate, memberExpression);
            var constantExpression = Expression.Constant(simplePredicate.ComparedValue, memberType);
            return TransformToExpression(simplePredicate.ValueComparingOperator, memberExpression, constantExpression);
        }

        private static Type GetMemberType(SimplePredicate simplePredicate, MemberExpression memberExpression)
        {
            return memberExpression.Member.DeclaringType?.GetProperty(simplePredicate.TargetPropertyName)?.PropertyType;
        }

        private static Expression TransformToExpression(ValueComparingOperator comparingOperator, MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            if (!Expressions.ContainsKey(comparingOperator))
            {
                throw new InvalidOperationException($"Transformation of value comparing operator: {comparingOperator} to expression is not supported!");
            }
            return Expressions[comparingOperator].Invoke(memberExpression, constantExpression);
        }
    }
}
