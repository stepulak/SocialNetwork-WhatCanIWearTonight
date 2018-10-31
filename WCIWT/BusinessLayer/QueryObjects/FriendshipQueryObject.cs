using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
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
    public class FriendshipQueryObject : QueryObjectBase<FriendshipDto, Friendship, FriendshipFilterDto, IQuery<Friendship>>
    {
        public FriendshipQueryObject(IMapper mapper, IQuery<Friendship> query) : base(mapper, query)
        {
        }

        protected override IQuery<Friendship> ApplyWhereClause(IQuery<Friendship> query, FriendshipFilterDto filter)
        {
            return filter.UserId == null ? query : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private CompositePredicate CreateCompositePredicateFromFilter(FriendshipFilterDto filter)
        {
            List<IPredicate> predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.Applicant), ValueComparingOperator.Equal, filter.UserId),
                new SimplePredicate(nameof(Friendship.Recipient), ValueComparingOperator.Equal, filter.UserId)
            };

            return new CompositePredicate(predicates, LogicalOperator.OR);
        }
    }
}
