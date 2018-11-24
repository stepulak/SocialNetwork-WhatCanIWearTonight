using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.PostReplys
{
    public interface IPostReplyService
    {
        Task<QueryResultDto<PostReplyDto, PostReplyFilterDto>> ListPostReplyAsync(PostReplyFilterDto filter);
    }
}
