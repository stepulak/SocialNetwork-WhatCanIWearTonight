using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Votes
{
    public interface IVoteService
    {
        Task<QueryResultDto<VoteDto, VoteFilterDto>> ListVoteAsync(VoteFilterDto filter);
    }
}
