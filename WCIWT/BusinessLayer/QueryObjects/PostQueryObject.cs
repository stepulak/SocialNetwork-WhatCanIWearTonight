﻿using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects.Common;
using EntityDatabase;
using WCIWT.Infrastructure.Query;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;
using Gender = EntityDatabase.Gender;
using PostVisibility = EntityDatabase.PostVisibility;

namespace BusinessLayer.QueryObjects
{
    public class PostQueryObject : QueryObjectBase<PostDto, Post, PostFilterDto, IQuery<Post>>
    {
        public PostQueryObject(IMapper mapper, IQuery<Post> query) : base(mapper, query)
        {
        }

        protected override IQuery<Post> ApplyWhereClause(IQuery<Post> query, PostFilterDto filter)
        {
            return query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private CompositePredicate CreateCompositePredicateFromFilter(PostFilterDto filter)
        {
            List<IPredicate> predicates = new List<IPredicate>();
            if (filter.UserId != Guid.Empty)
            {
                predicates.Add(new SimplePredicate(nameof(Post.UserId), ValueComparingOperator.Equal, filter.UserId));
            }
            if (filter.IncludePrivatePosts)
            {
                predicates.Add(CreateVisibilityRestrictionPostPredicates(filter));
            }
            else
            {
                predicates.Add(new SimplePredicate(nameof(Post.Visibility), ValueComparingOperator.Equal, PostVisibility.Public));
            }

            predicates.Add(CreateGenderRestrictionPredicate(filter));
            predicates.Add(CreateAgeRestrictionPredicate(filter));
            return new CompositePredicate(predicates, LogicalOperator.AND);
        }

        private IPredicate CreateVisibilityRestrictionPostPredicates(PostFilterDto filter)
        {
            var friendsOnlyPostsPredicates = new List<IPredicate>
            {
               new SimplePredicate(nameof(Post.Visibility), ValueComparingOperator.Equal, PostVisibility.FriendsOnly)
            };
            
            if (filter.PostUserIds != null && filter.PostUserIds.Count > 0)
            {
                friendsOnlyPostsPredicates.Add(CreatePostUsernameRestrictionsPredicate(filter));
            }
             
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.Visibility), ValueComparingOperator.Equal, PostVisibility.Public),
                new CompositePredicate(friendsOnlyPostsPredicates, LogicalOperator.AND)
            };
            var post = new CompositePredicate(predicates, LogicalOperator.OR);
            return post;
        }

        private IPredicate CreatePostUsernameRestrictionsPredicate(PostFilterDto filter) {
            var predicates = new List<IPredicate>();
            foreach (Guid userId in filter.PostUserIds)
            {
                predicates.Add(new SimplePredicate(nameof(Post.UserId), ValueComparingOperator.Equal, userId));
            }
            return new CompositePredicate(predicates, LogicalOperator.OR);
        }

        private IPredicate CreateAgeRestrictionPredicate(PostFilterDto filter)
        {
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.HasAgeRestriction), ValueComparingOperator.Equal, false) // add posts who has no age restriction
            };
            var innerPredicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.AgeRestrictionTo), ValueComparingOperator.LessThanOrEqual, filter.UserAge),
                new SimplePredicate(nameof(Post.AgeRestrictionFrom), ValueComparingOperator.GreaterThanOrEqual, filter.UserAge)
            };
            predicates.Add(new CompositePredicate(innerPredicates, LogicalOperator.AND));
            return new CompositePredicate(predicates, LogicalOperator.OR);
        }

        private IPredicate CreateGenderRestrictionPredicate(PostFilterDto filter)
        {
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, GetGenderRetrictionEnumFromFilter(filter)), // filter by user's gender
                new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, Gender.NoInformation) // and add posts with no gender restriction
            };
            return new CompositePredicate(predicates, LogicalOperator.OR);
        }

        private Gender GetGenderRetrictionEnumFromFilter(PostFilterDto filter)
        {
            switch (filter.GenderRestriction)
            {
                case DataTransferObjects.Gender.Male: return Gender.Male;
                case DataTransferObjects.Gender.Female: return Gender.Female;
                default: return Gender.NoInformation;
            }
        }
    }
}
