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
    public class ImageQueryObject : QueryObjectBase<ImageDto, Image, ImageFilterDto, IQuery<Image>>
    {
        public ImageQueryObject(IMapper mapper, IQuery<Image> query) : base(mapper, query)
        {
        }

        protected override IQuery<Image> ApplyWhereClause(IQuery<Image> query, ImageFilterDto filter)
        {
            return filter.PostId == Guid.Empty 
                ? query 
                : query.Where(new SimplePredicate(nameof(Image.PostId), ValueComparingOperator.Equal, filter.PostId));
        }
    }
}
