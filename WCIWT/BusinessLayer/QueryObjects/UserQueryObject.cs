using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects.Common;
using EntityDatabase;
using WCIWT.Infrastructure.Query;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;
using Gender = BusinessLayer.DataTransferObjects.Gender;

namespace BusinessLayer.QueryObjects
{
    public class UserQueryObject: QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>>
    {
        public UserQueryObject(IMapper mapper, IQuery<User> query): base(mapper, query) { }

        protected override IQuery<User> ApplyWhereClause(IQuery<User> query, UserFilterDto filter)
        {
            return filter.Username != null
                   && filter.Email != null
                   && filter.Gender == Gender.NoInformation 
                   && filter.BornBefore == DateTime.MinValue 
                   && filter.BornAfter == DateTime.MinValue
                ? query
                : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private CompositePredicate CreateCompositePredicateFromFilter(UserFilterDto filter)
        {
            List<IPredicate> predicates = new List<IPredicate>();

            if (filter.Username != null)
            {
                predicates.Add(new SimplePredicate(nameof(User.Username), ValueComparingOperator.Equal, filter.Username));
            }
            if (filter.Email != null)
            {
                predicates.Add(new SimplePredicate(nameof(User.Email), ValueComparingOperator.Equal, filter.Email));
            }
            if (filter.Gender != Gender.NoInformation)
            {
                predicates.Add(new SimplePredicate(nameof(User.Gender), ValueComparingOperator.Equal, filter.Gender));
            }
            if (filter.BornBefore != DateTime.MinValue)
            {
                predicates.Add(new SimplePredicate(nameof(User.Birthdate), ValueComparingOperator.LessThan, filter.BornBefore));
            }
            if (filter.BornAfter != DateTime.MinValue)
            {
                predicates.Add(new SimplePredicate(nameof(User.Birthdate), ValueComparingOperator.GreaterThan, filter.BornAfter));
            }

            return new CompositePredicate(predicates, LogicalOperator.AND);
        }
    }
}
