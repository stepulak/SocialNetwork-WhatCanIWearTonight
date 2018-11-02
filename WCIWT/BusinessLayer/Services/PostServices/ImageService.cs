﻿using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Common;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.PostServices
{
    public class ImageService : CrudQueryServiceBase<Image, ImageDto, ImageFilterDto>, IImageService
    {
        private readonly QueryObjectBase<ImageDto, Image, ImageFilterDto, IQuery<Image>> imageQueryObject;


        public ImageService(IMapper mapper, IRepository<Image> repository, ImageQueryObject query,
            QueryObjectBase<ImageDto, Image, ImageFilterDto, IQuery<Image>> imageQueryObject)
            : base(mapper, repository, query)
        {
            this.imageQueryObject = imageQueryObject;
        }

        public async Task<QueryResultDto<ImageDto, ImageFilterDto>> ListImageAsync(ImageFilterDto filter)
        {
            return await imageQueryObject.ExecuteQuery(filter); 
        }

        protected override Task<Image> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Post");
        }
    }
}
