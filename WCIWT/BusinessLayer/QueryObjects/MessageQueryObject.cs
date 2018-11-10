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
    public class MessageQueryObject : QueryObjectBase<MessageDto, Message, MessageFilterDto, IQuery<Message>>
    {
        public MessageQueryObject(IMapper mapper, IQuery<Message> query) : base(mapper, query)
        {
        }

        protected override IQuery<Message> ApplyWhereClause(IQuery<Message> query, MessageFilterDto filter)
        {
            return filter.UserId == null ? query : query.Where(CreateCompositePredicateFromFilter(filter));
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
