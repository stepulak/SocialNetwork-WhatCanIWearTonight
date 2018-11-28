using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using EntityDatabase;

namespace BusinessLayer.Services.Posts
{
    public interface IPostService
    {
        Task<PostDto> GetAsync(Guid id, bool withIncludes = true);
        Guid Create(PostDto entityDto);
        Task Update(PostDto entityDto);
        void Delete(Guid entityId);
        Task<QueryResultDto<PostDto, PostFilterDto>> ListAllAsync();
        Task<QueryResultDto<PostDto, PostFilterDto>> ListPostAsync(PostFilterDto filter);
        Task<QueryResultDto<PostDto, PostFilterDto>> ListUserOwnedPosts(Guid userId);
        Task<QueryResultDto<PostDto, PostFilterDto>> ListPostsAvailableForUser(Guid userId, PostFilterDto filter);

    }
}
