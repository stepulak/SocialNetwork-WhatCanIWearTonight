using AutoMapper;
using BusinessLayer.DataTransferObjects.Filters.Common;
using EntityDatabase;
using System.Collections.Generic;
using WCIWT.Infrastructure.Query;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class PostQueryObject : QueryObjectBase<PostDto, Post, PostFilterDto, IQuery<Post>>
    {
        public PostQueryObject(IMapper mapper, IQuery<Post> query) : base(mapper, query)
        {
        }

        protected override IQuery<Post> ApplyWhereClause(IQuery<Post> query, PostFilterDto filter)
        {
            return filter.UserId == null && filter.UserAge != -1 && filter.GenderRestriction == Gender.NoInformation && filter.Visibility == PostVisibility.Public
                ? query
                : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private CompositePredicate CreateCompositePredicateFromFilter(PostFilterDto filter)
        {
            List<IPredicate> predicates = new List<IPredicate>();

            if (filter.UserAge != -1)
            {
                predicates.Add(CreateAgeRestrictionPredicate(filter));
            }
            if (filter.UserId != null)
            {
                predicates.Add(new SimplePredicate(nameof(Post.UserId), ValueComparingOperator.Equal, filter.UserId));
            }
            if (filter.GenderRestriction != Gender.NoInformation)
            {
                predicates.Add(CreateGenderRestrictionPredicate(filter));
            }
            if (filter.Visibility != PostVisibility.Public)
            {
                predicates.Add(new SimplePredicate(nameof(Post.Visibility), ValueComparingOperator.Equal, filter.Visibility));
            }

            return new CompositePredicate(predicates, LogicalOperator.AND);
        }

        private IPredicate CreateAgeRestrictionPredicate(PostFilterDto filter)
        {
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.HasAgeRestriction), ValueComparingOperator.Equal, false) // add posts who has no age restriction
            };
            var innerPredicates = new List<IPredicate>()
            {
                new SimplePredicate(nameof(Post.AgeRestrictionTo), ValueComparingOperator.GreaterThanOrEqual, filter.UserAge),
                new SimplePredicate(nameof(Post.AgeRestrictionFrom), ValueComparingOperator.LessThanOrEqual, filter.UserAge)
            };
            predicates.Add(new CompositePredicate(innerPredicates, LogicalOperator.AND));
            return new CompositePredicate(predicates, LogicalOperator.OR);
        }

        private IPredicate CreateGenderRestrictionPredicate(PostFilterDto filter)
        {
            var predicates = new List<IPredicate>
            {
                new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, filter.GenderRestriction), // filter by user's gender
                new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, Gender.NoInformation) // and add posts with no gender restriction
            };
            return new CompositePredicate(predicates, LogicalOperator.OR);
        }
    }
}
