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
    public class VoteQueryObject : QueryObjectBase<VoteDto, Vote, VoteFilterDto, IQuery<Vote>>
    {
        public VoteQueryObject(IMapper mapper, IQuery<Vote> query) : base(mapper, query)
        {
        }

        protected override IQuery<Vote> ApplyWhereClause(IQuery<Vote> query, VoteFilterDto filter)
        {
            return filter.ImageId == null ? query : query.Where(new SimplePredicate(nameof(Image.Id), ValueComparingOperator.Equal, filter.ImageId));
        }
    }
}
