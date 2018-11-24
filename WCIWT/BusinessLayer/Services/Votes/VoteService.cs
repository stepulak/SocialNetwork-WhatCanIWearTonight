using System;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Common;
using BusinessLayer.Services.Common;
using BusinessLayer.Services.PostServices;
using EntityDatabase;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.Votes
{
    public class VoteService : CrudQueryServiceBase<Vote, VoteDto, VoteFilterDto>, IVoteService
    {
        private readonly QueryObjectBase<VoteDto, Vote, VoteFilterDto, IQuery<Vote>> voteQueryObject;

        public VoteService(IMapper mapper, IRepository<Vote> repository, VoteQueryObject query,
            QueryObjectBase<VoteDto, Vote, VoteFilterDto, IQuery<Vote>> voteQueryObject)
            : base(mapper, repository, query)
        {
            this.voteQueryObject = voteQueryObject;
        }

        public async Task<QueryResultDto<VoteDto, VoteFilterDto>> ListVoteAsync(VoteFilterDto filter)
        {
            return await voteQueryObject.ExecuteQuery(filter);
        }

        protected override Task<Vote> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Image", "User");
        }
    }
}
