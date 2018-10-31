using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using WCIWT.Infrastructure;

namespace BusinessLayer.Services.PostServices
{
    public class ImageService : CrudQueryServiceBase<Image, ImageDto, ImageFilterDto>
    {
        public ImageService(IMapper mapper, IRepository<Image> repository, ImageQueryObject query)
            : base(mapper, repository, query)
        {
        }

        protected override Task<Image> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Post");
        }
    }
}
