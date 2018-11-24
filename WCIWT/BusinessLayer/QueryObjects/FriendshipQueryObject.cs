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
    public class FriendshipQueryObject : QueryObjectBase<FriendshipDto, Friendship, FriendshipFilterDto, IQuery<Friendship>>
    {
        public FriendshipQueryObject(IMapper mapper, IQuery<Friendship> query) : base(mapper, query)
        {
        }

        protected override IQuery<Friendship> ApplyWhereClause(IQuery<Friendship> query, FriendshipFilterDto filter)
        {
            return filter.UserA == Guid.Empty 
                   && filter.UserB == Guid.Empty 
                ? query 
                : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private IPredicate CreateCompositePredicateFromFilter(FriendshipFilterDto filter)
        {
            if (filter.UserA == Guid.Empty)
            {
                return CreatePredicateForOneUser(filter.UserB);
            }
            if (filter.UserB == Guid.Empty)
            {
                return CreatePredicateForOneUser(filter.UserA);
            }
            
            // Both users must be in frienship structure, doesn't matter who is sender or recipient
            return CreatePredicateForBothUsers(filter.UserA, filter.UserB);
        }

        private IPredicate CreatePredicateForOneUser(Guid userId)
        {
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.ApplicantId), ValueComparingOperator.Equal, userId),
                new SimplePredicate(nameof(Friendship.RecipientId), ValueComparingOperator.Equal, userId)
            };
            return new CompositePredicate(predicates, LogicalOperator.OR);
        }
        
        private IPredicate CreatePredicateForBothUsers(Guid userAId, Guid userBId)
        {
            var userAIsApplicantPredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.ApplicantId), ValueComparingOperator.Equal, userAId),
                new SimplePredicate(nameof(Friendship.RecipientId), ValueComparingOperator.Equal, userBId)
            };
            var userBIsApplicantPredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Friendship.ApplicantId), ValueComparingOperator.Equal, userBId),
                new SimplePredicate(nameof(Friendship.RecipientId), ValueComparingOperator.Equal, userAId)
            };
            
            var compositeOfUserAIsApplicant = new CompositePredicate(userAIsApplicantPredicates, LogicalOperator.AND);
            var compositeOfUserBIsApplicant = new CompositePredicate(userBIsApplicantPredicates, LogicalOperator.AND);
            
            return new CompositePredicate(new List<IPredicate>
            {
                compositeOfUserAIsApplicant,
                compositeOfUserBIsApplicant
            }, LogicalOperator.OR);
        }
    }
}
