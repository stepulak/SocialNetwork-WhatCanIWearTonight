using BusinessLayer.QueryObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.QueryObjects.Common;
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
            return string.IsNullOrWhiteSpace(filter.Tag) ? query : query.Where(new SimplePredicate(nameof(Hashtag.Tag), ValueComparingOperator.Equal, filter.Tag));
        }
    }
}
