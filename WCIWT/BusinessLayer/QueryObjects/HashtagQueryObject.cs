using BusinessLayer.QueryObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using EntityDatabase;
using BusinessLayer.DataTransferObjects.Filters;
using WCIWT.Infrastructure.Query;
using AutoMapper;

using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;

namespace BusinessLayer.QueryObjects
{
    public class HashtagQueryObject : QueryObjectBase<HashtagDto, Hashtag, HashtagFilterDto, IQuery<Hashtag>>
    {
        public HashtagQueryObject(IMapper mapper, IQuery<Hashtag> query) : base(mapper, query)
        {
        }

        protected override IQuery<Hashtag> ApplyWhereClause(IQuery<Hashtag> query, HashtagFilterDto filter)
        {
            return string.IsNullOrWhiteSpace(filter.Tag) && filter.PostId == null 
                ? query 
                : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private CompositePredicate CreateCompositePredicateFromFilter(HashtagFilterDto filter)
        {
            var predicates = new List<IPredicate>();
            if (!string.IsNullOrWhiteSpace(filter.Tag))
            {
                predicates.Add(new SimplePredicate(nameof(Hashtag.Tag), ValueComparingOperator.Equal, filter.Tag));
            }
            if (filter.PostId != null)
            {
                predicates.Add(new SimplePredicate(nameof(Hashtag.PostId), ValueComparingOperator.Equal, filter.PostId));
            }
            return new CompositePredicate(predicates, LogicalOperator.AND);
        }
    }
}
