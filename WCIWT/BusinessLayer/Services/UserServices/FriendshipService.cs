using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using WCIWT.Infrastructure;

namespace BusinessLayer.Services.UserServices
{
    public class FriendshipService : CrudQueryServiceBase<Friendship, FriendshipDto, FriendshipFilterDto>
    {
        public FriendshipService(IMapper mapper, IRepository<Friendship> repository, FriendshipQueryObject query)
            : base(mapper, repository, query)
        {
        }

        protected override Task<Friendship> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, "Applicant", "Recipient");
        }
    }
}

