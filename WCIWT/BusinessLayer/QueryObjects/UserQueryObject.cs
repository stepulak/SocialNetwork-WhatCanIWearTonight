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
    public class UserQueryObject: QueryObjectBase<UserDto, UserQueryObject, UserFilterDto, IQuery<User>>
    {
        public CustomerQueryObject(IMapper mapper, IQuery<User> query): base(mapper, query) { }

        protected override IQuery<UserQueryObject> ApplyWhereClause(IQuery<UserQueryObject> query, UserFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Email))
            {
                return query;
            }

            return query.Where(new SimplePredicate(nameof(User.Email), ValueComparingOperator.Equal, filter.Email));
        }

    }
}
