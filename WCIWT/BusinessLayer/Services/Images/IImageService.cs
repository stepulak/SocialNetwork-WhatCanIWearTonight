using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Images
{
    public interface IImageService
    {
        Task<QueryResultDto<ImageDto, ImageFilterDto>> ListImageAsync(ImageFilterDto filter);
    }
}
