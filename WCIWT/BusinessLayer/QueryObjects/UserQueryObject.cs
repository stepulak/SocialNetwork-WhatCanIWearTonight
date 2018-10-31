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

namespace BusinessLayer.QueryObjects
{
    public class UserQueryObject: QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>>
    {
        public UserQueryObject(IMapper mapper, IQuery<User> query): base(mapper, query) { }

        protected override IQuery<User> ApplyWhereClause(IQuery<User> query, UserFilterDto filter)
        {
            return string.IsNullOrWhiteSpace(filter.Username)
                ? query
                : query.Where(new SimplePredicate(nameof(User.Username), ValueComparingOperator.Equal, filter.Username));
        }
    }
}
