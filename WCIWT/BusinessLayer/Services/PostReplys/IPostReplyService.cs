using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.PostReplys
{
    public interface IPostReplyService
    {
        Task<PostReplyDto> GetAsync(Guid id, bool withIncludes = true);
        Guid Create(PostReplyDto entityDto);
        Task Update(PostReplyDto entityDto);
        void Delete(Guid entityId);
        Task<QueryResultDto<PostReplyDto, PostReplyFilterDto>> ListAllAsync();
        Task<QueryResultDto<PostReplyDto, PostReplyFilterDto>> ListPostReplyAsync(PostReplyFilterDto filter);
    }
}
