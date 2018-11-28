using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Common;
using BusinessLayer.Services.Common;
using BusinessLayer.Services.PostServices;
using EntityDatabase;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.Images
{
    public class ImageService : CrudQueryServiceBase<Image, ImageDto, ImageFilterDto>, IImageService
    {
        public ImageService(IMapper mapper, IRepository<Image> repository, QueryObjectBase<ImageDto, Image, ImageFilterDto, IQuery<Image>> imageQueryObject)
            : base(mapper, repository, imageQueryObject)
        {
        }

        public async Task<List<ImageDto>> GetSortedImagesByVotes(Guid postId)
        {
            var images = await ListImageAsync(new ImageFilterDto { PostId = postId });
            return images.Items.OrderBy(i => i.LikesCount - i.DislikesCount).ToList();
        }

        public async Task<QueryResultDto<ImageDto, ImageFilterDto>> ListImageAsync(ImageFilterDto filter)
        {
            return await Query.ExecuteQuery(filter); 
        }

        protected override Task<Image> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Post");
        }
    }
}
