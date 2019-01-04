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
            return filter == null ? query : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private IPredicate CreateCompositePredicateFromFilter(MessageFilterDto filter)
        {
            var predicates = new List<IPredicate>();
            predicates.Add(filter.CareAboutRole
                ? CareAboutRolePredicates(filter)
                : DontCareAboutRolePredicates(filter));
            if (filter.UnseenOnly)
            {
                predicates.Add(new SimplePredicate(nameof(Message.Seen), ValueComparingOperator.Equal, false));
            }
            return new CompositePredicate(predicates, LogicalOperator.AND);
        }

        private IPredicate CareAboutRolePredicates(MessageFilterDto filter)
        {
            var predicates = new List<IPredicate>();
            predicates.Add(new SimplePredicate(nameof(Message.UserSenderId), ValueComparingOperator.Equal, filter.Sender));
            predicates.Add(new SimplePredicate(nameof(Message.UserReceiverId), ValueComparingOperator.Equal, filter.Receiver));
            return new CompositePredicate(predicates, LogicalOperator.AND);

        }

        private IPredicate DontCareAboutRolePredicates(MessageFilterDto filter)
        {
            var predicates = new List<IPredicate>();

            var opt1 = new List<IPredicate>();
            if (filter.Sender != Guid.Empty)
            {
                opt1.Add(new SimplePredicate(nameof(Message.UserSenderId), ValueComparingOperator.Equal, filter.Sender));
            }
            if (filter.Receiver != Guid.Empty)
            {
                opt1.Add(new SimplePredicate(nameof(Message.UserReceiverId), ValueComparingOperator.Equal, filter.Receiver));
            }
            predicates.Add(new CompositePredicate(opt1, LogicalOperator.AND));

            var opt2 = new List<IPredicate>();
            if (filter.Receiver != Guid.Empty)
            {
                opt2.Add(new SimplePredicate(nameof(Message.UserSenderId), ValueComparingOperator.Equal, filter.Receiver));
            }
            if (filter.Sender != Guid.Empty)
            {
                opt2.Add(new SimplePredicate(nameof(Message.UserReceiverId), ValueComparingOperator.Equal, filter.Sender));
            }
            predicates.Add(new CompositePredicate(opt2, LogicalOperator.AND));


            return new CompositePredicate(predicates, LogicalOperator.OR);
        }
    }
}
