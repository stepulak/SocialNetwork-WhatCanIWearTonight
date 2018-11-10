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
    public class PostReplyQueryObject : QueryObjectBase<PostReplyDto, PostReply, PostReplyFilterDto, IQuery<PostReply>>
    {
        public PostReplyQueryObject(IMapper mapper, IQuery<PostReply> query) : base(mapper, query)
        {
        }

        protected override IQuery<PostReply> ApplyWhereClause(IQuery<PostReply> query, PostReplyFilterDto filter)
        {
            return filter.PostId == null ? query : query.Where(new SimplePredicate(nameof(PostReply.PostId), ValueComparingOperator.Equal, filter.PostId));
        }
    }
}
