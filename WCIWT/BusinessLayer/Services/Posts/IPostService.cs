using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Posts
{
    public interface IPostService
    {
        Task<QueryResultDto<PostDto, PostFilterDto>> ListPostAsync(PostFilterDto filter);
    }
}
