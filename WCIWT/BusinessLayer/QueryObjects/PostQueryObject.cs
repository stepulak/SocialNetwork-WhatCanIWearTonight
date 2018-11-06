using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.DataTransferObjects.Filters.Common;
using EntityDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.Query;
using WCIWT.Infrastructure.Query.Predicates;
using WCIWT.Infrastructure.Query.Predicates.Operators;
using Gender = BusinessLayer.DataTransferObjects.Gender;
using PostVisibility = BusinessLayer.DataTransferObjects.PostVisibility;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class PostQueryObject : QueryObjectBase<PostDto, Post, PostFilterDto, IQuery<Post>>
    {
        public PostQueryObject(IMapper mapper, IQuery<Post> query) : base(mapper, query)
        {
        }

        protected override IQuery<Post> ApplyWhereClause(IQuery<Post> query, PostFilterDto filter)
        {
            return filter.UserId == null && !filter.HasAgeRestriction && filter.GenderRestriction == Gender.NoInformation && filter.Visibility == PostVisibility.Public
                ? query
                : query.Where(CreateCompositePredicateFromFilter(filter));
        }

        private CompositePredicate CreateCompositePredicateFromFilter(PostFilterDto filter)
        {
            List<IPredicate> predicates = new List<IPredicate>();

            if (filter.HasAgeRestriction)
            {
                predicates.Add(CreateAgeRestrictionPredicate(filter));
            }
            if (filter.UserId != null)
            {
                predicates.Add(new SimplePredicate(nameof(Post.UserId), ValueComparingOperator.Equal, filter.UserId));
            }
            if (filter.GenderRestriction != Gender.NoInformation)
            {
                var innerPredicates = new List<IPredicate>
                {
                    new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, filter.GenderRestriction), // filter by user's gender
                    new SimplePredicate(nameof(Post.GenderRestriction), ValueComparingOperator.Equal, Gender.NoInformation) // and add posts with no gender restriction
                };
                predicates.Add(new CompositePredicate(innerPredicates, LogicalOperator.OR));
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
                new SimplePredicate(nameof(Post.HasAgeRestriction), ValueComparingOperator.Equal, false)
            };

            if (filter.AgeRestrictionFrom > 0 && filter.AgeRestrictionTo <= 0)
            {
                predicates.Add(new SimplePredicate(nameof(Post.AgeRestrictionFrom), ValueComparingOperator.GreaterThanOrEqual, filter.AgeRestrictionFrom));
            }
            else if (filter.AgeRestrictionTo > 0 && filter.AgeRestrictionFrom <= 0)
            {
                predicates.Add(new SimplePredicate(nameof(Post.AgeRestrictionTo), ValueComparingOperator.LessThan, filter.AgeRestrictionTo));
            }
            else
            {
                var innerPredicates = new List<IPredicate>()
                {
                    new SimplePredicate(nameof(Post.AgeRestrictionFrom), ValueComparingOperator.GreaterThanOrEqual, filter.AgeRestrictionFrom),
                    new SimplePredicate(nameof(Post.AgeRestrictionTo), ValueComparingOperator.LessThanOrEqual, filter.AgeRestrictionTo)
                };
                predicates.Add(new CompositePredicate(innerPredicates, LogicalOperator.AND));
            }

            return new CompositePredicate(predicates, LogicalOperator.OR);
        }
    }
}
