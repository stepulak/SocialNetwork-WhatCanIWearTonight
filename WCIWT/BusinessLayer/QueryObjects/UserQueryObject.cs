using AutoMapper;
using BusinessLayer.DataTransferObjects;
using EntityDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.QueryObjects.Common;
using WCIWT.Infrastructure.Query;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;
using Gender = BusinessLayer.DataTransferObjects.Gender;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class UserQueryObject: QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>>
    {
        public UserQueryObject(IMapper mapper, IQuery<User> query): base(mapper, query) { }

        protected override IQuery<User> ApplyWhereClause(IQuery<User> query, UserFilterDto filter)
        {
            return string.IsNullOrWhiteSpace(filter.Username) 
                   && string.IsNullOrWhiteSpace(filter.Email) 
                   && filter.Gender == Gender.NoInformation 
                   && filter.BornBefore == DateTime.MinValue 
                   && filter.BornAfter == DateTime.MinValue
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
            if (!string.IsNullOrWhiteSpace(filter.Email))
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
