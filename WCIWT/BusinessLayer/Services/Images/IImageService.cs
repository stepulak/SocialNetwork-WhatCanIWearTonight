using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Images
{
    public interface IImageService
    {
        Task<ImageDto> GetAsync(Guid id, bool withIncludes = true);
        Guid Create(ImageDto entityDto);
        Task Update(ImageDto entityDto);
        void Delete(Guid entityId);
        Task<QueryResultDto<ImageDto, ImageFilterDto>> ListAllAsync();
        Task<QueryResultDto<ImageDto, ImageFilterDto>> ListImageAsync(ImageFilterDto filter);
        Task<List<ImageDto>> GetSortedImagesByVotes(Guid postId);
        
    }
}
