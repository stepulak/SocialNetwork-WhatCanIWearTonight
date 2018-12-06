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

namespace BusinessLayer.QueryObjects
{
    public class VoteQueryObject : QueryObjectBase<VoteDto, Vote, VoteFilterDto, IQuery<Vote>>
    {
        public VoteQueryObject(IMapper mapper, IQuery<Vote> query) : base(mapper, query)
        {
        }

        protected override IQuery<Vote> ApplyWhereClause(IQuery<Vote> query, VoteFilterDto filter)
        {
            return filter.ImageId == Guid.Empty && filter.UserId == Guid.Empty ? query : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private IPredicate CreateCompositePredicateFromFilter(VoteFilterDto filter)
        {
            // either one of UserId or ImageId
            if (filter.UserId != Guid.Empty && filter.ImageId == Guid.Empty)
            {
                return new SimplePredicate(nameof(Vote.UserId), ValueComparingOperator.Equal, filter.UserId);
            }
            if (filter.ImageId != Guid.Empty && filter.UserId == Guid.Empty)
            {
                return new SimplePredicate(nameof(Vote.ImageId), ValueComparingOperator.Equal, filter.ImageId);
            }
            // Or both
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Vote.ImageId), ValueComparingOperator.Equal, filter.ImageId),
                new SimplePredicate(nameof(Vote.UserId), ValueComparingOperator.Equal, filter.UserId)
            };
            return new CompositePredicate(predicates, LogicalOperator.AND);
        }
    }
}
