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
    public class VoteQueryObject : QueryObjectBase<VoteDto, Vote, VoteFilterDto, IQuery<Vote>>
    {
        public VoteQueryObject(IMapper mapper, IQuery<Vote> query) : base(mapper, query)
        {
        }

        protected override IQuery<Vote> ApplyWhereClause(IQuery<Vote> query, VoteFilterDto filter)
        {
            return filter.ImageId == null && filter.UserId == null ? query : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private IPredicate CreateCompositePredicateFromFilter(VoteFilterDto filter)
        {
            // either one of UserId or ImageId
            if (filter.UserId == null)
            {
                return new SimplePredicate(nameof(Image.Id), ValueComparingOperator.Equal, filter.ImageId);
            }
            if (filter.ImageId == null)
            {
                return new SimplePredicate(nameof(User.Id), ValueComparingOperator.Equal, filter.UserId);
            }
            // Or both
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Image.Id), ValueComparingOperator.Equal, filter.ImageId),
                new SimplePredicate(nameof(User.Id), ValueComparingOperator.Equal, filter.UserId),
            };
            return new CompositePredicate(predicates, LogicalOperator.AND);
        }
    }
}
