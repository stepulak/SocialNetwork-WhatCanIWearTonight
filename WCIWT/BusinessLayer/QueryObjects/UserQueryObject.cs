using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.QueryObjects.Common;
using EntityDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return string.IsNullOrWhiteSpace(filter.Username) && filter.Gender == Gender.NoInformation && filter.BornBefore == null && filter.BornAfter == null
                ? query
                : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private CompositePredicate CreateCompositePredicateFromFilter(UserFilterDto filter)
        {
            List<IPredicate> predicates = new List<IPredicate>();

            if (!string.IsNullOrWhiteSpace(filter.Username))
            {
                predicates.Add(new SimplePredicate(nameof(User.Username), ValueComparingOperator.Equal, filter.Username));
            }
            if (filter.Gender != Gender.NoInformation)
            {
                predicates.Add(new SimplePredicate(nameof(User.Gender), ValueComparingOperator.Equal, filter.Gender));
            }
            if (filter.BornBefore != null)
            {
                predicates.Add(new SimplePredicate(nameof(User.Birthdate), ValueComparingOperator.LessThan, filter.BornBefore));
            }
            if (filter.BornAfter != null)
            {
                predicates.Add(new SimplePredicate(nameof(User.Birthdate), ValueComparingOperator.GreaterThan, filter.BornAfter));
            }

            return new CompositePredicate(predicates, LogicalOperator.AND);
        }
    }
}
