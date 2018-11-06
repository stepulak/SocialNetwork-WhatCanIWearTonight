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

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class ImageQueryObject : QueryObjectBase<ImageDto, Image, ImageFilterDto, IQuery<Image>>
    {
        public ImageQueryObject(IMapper mapper, IQuery<Image> query) : base(mapper, query)
        {
        }

        protected override IQuery<Image> ApplyWhereClause(IQuery<Image> query, ImageFilterDto filter)
        {
            return filter.PostId == null ? query : query.Where(new SimplePredicate(nameof(Image.PostId), ValueComparingOperator.Equal, filter.PostId));
        }
    }
}
