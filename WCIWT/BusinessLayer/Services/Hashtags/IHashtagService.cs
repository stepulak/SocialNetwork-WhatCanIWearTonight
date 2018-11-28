using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using EntityDatabase;

namespace BusinessLayer.Services.PostServices
{
    public interface IHashtagService
    {
        Task<HashtagDto> GetAsync(Guid id, bool withIncludes = true);
        Guid Create(HashtagDto entityDto);
        Task Update(HashtagDto entityDto);
        void Delete(Guid entityId);
        Task<QueryResultDto<HashtagDto, HashtagFilterDto>> ListAllAsync();
        Task<QueryResultDto<HashtagDto, HashtagFilterDto>> ListHashtagAsync(HashtagFilterDto filter);
    }
}