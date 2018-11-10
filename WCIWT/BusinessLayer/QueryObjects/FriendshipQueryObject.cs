using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
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

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class FriendshipQueryObject : QueryObjectBase<FriendshipDto, Friendship, FriendshipFilterDto, IQuery<Friendship>>
    {
        public FriendshipQueryObject(IMapper mapper, IQuery<Friendship> query) : base(mapper, query)
        {
        }

        protected override IQuery<Friendship> ApplyWhereClause(IQuery<Friendship> query, FriendshipFilterDto filter)
        {
            return filter.UserA == null && filter.UserB == null ? query : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private IPredicate CreateCompositePredicateFromFilter(FriendshipFilterDto filter)
        {
            if (filter.UserA == null)
            {
                return CreatePredicateForOneUser(filter.UserB);
            }
            if (filter.UserB == null)
            {
                return CreatePredicateForOneUser(filter.UserA);
            }
            var predicates = new List<IPredicate>
            {
                // Both users must be in frienship structure, doesn't matter who is sender or recipient
                CreatePredicateForOneUser(filter.UserA),
                CreatePredicateForOneUser(filter.UserB)
            };
            return new CompositePredicate(predicates, LogicalOperator.AND);
        }

        private IPredicate CreatePredicateForOneUser(Guid userId)
        {
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.Applicant), ValueComparingOperator.Equal, userId),
                new SimplePredicate(nameof(Friendship.Recipient), ValueComparingOperator.Equal, userId)
            };
            return new CompositePredicate(predicates, LogicalOperator.OR);
        }
    }
}
