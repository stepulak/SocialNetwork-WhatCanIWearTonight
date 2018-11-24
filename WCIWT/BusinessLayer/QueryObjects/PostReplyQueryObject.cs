using System;
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
    public class PostReplyQueryObject : QueryObjectBase<PostReplyDto, PostReply, PostReplyFilterDto, IQuery<PostReply>>
    {
        public PostReplyQueryObject(IMapper mapper, IQuery<PostReply> query) : base(mapper, query)
        {
        }

        protected override IQuery<PostReply> ApplyWhereClause(IQuery<PostReply> query, PostReplyFilterDto filter)
        {
            return filter.PostId == Guid.Empty
                ? query 
                : query.Where(new SimplePredicate(nameof(PostReply.PostId), ValueComparingOperator.Equal, filter.PostId));
        }
    }
}
