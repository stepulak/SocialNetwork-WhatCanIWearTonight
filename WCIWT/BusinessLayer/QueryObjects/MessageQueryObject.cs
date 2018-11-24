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
    public class MessageQueryObject : QueryObjectBase<MessageDto, Message, MessageFilterDto, IQuery<Message>>
    {
        public MessageQueryObject(IMapper mapper, IQuery<Message> query) : base(mapper, query)
        {
        }

        protected override IQuery<Message> ApplyWhereClause(IQuery<Message> query, MessageFilterDto filter)
        {
            return filter.UserId == Guid.Empty ? query : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private IPredicate CreateCompositePredicateFromFilter(MessageFilterDto filter)
        {
            if (filter.UserFilterType == MessageUserFilterType.Receiver)
            {
                return new SimplePredicate(nameof(Message.UserReceiverId), ValueComparingOperator.Equal, filter.UserId);
            }
            if (filter.UserFilterType == MessageUserFilterType.Sender)
            {
                return new SimplePredicate(nameof(Message.UserSenderId), ValueComparingOperator.Equal, filter.UserId);
            }
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Message.UserReceiverId), ValueComparingOperator.Equal, filter.UserId),
                new SimplePredicate(nameof(Message.UserSenderId), ValueComparingOperator.Equal, filter.UserId)
            };
            return new CompositePredicate(predicates, LogicalOperator.OR);
        }
    }
}
