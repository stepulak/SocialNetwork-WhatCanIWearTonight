using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using WCIWT.Infrastructure;

namespace BusinessLayer.Services.PostServices
{
    public class VoteService : CrudQueryServiceBase<Vote, VoteDto, VoteFilterDto>
    {
        public VoteService(IMapper mapper, IRepository<Vote> repository, VoteQueryObject query)
            : base(mapper, repository, query)
        {
        }

        protected override Task<Vote> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Image", "User");
        }
    }
}
