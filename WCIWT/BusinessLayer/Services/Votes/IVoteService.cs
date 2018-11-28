using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Votes
{
    public interface IVoteService
    {
        Task<VoteDto> GetAsync(Guid id, bool withIncludes = true);
        Guid Create(VoteDto entityDto);
        Task Update(VoteDto entityDto);
        void Delete(Guid entityId);
        Task<QueryResultDto<VoteDto, VoteFilterDto>> ListAllAsync();
        Task<QueryResultDto<VoteDto, VoteFilterDto>> ListVoteAsync(VoteFilterDto filter);
    }
}
