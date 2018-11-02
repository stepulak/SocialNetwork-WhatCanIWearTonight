using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.PostServices
{
    public interface IImageService
    {
        Task<QueryResultDto<ImageDto, ImageFilterDto>> ListImageAsync(ImageFilterDto filter);
    }
}
