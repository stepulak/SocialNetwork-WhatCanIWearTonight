using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.QueryObjects;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using WCIWT.Infrastructure;

namespace BusinessLayer.Services.UserServices
{
    public class UserService : CrudQueryServiceBase<User, UserDto, UserFilterDto>
    {
        public UserService(IMapper mapper, IRepository<User> repository, UserQueryObject query)
            : base(mapper, repository, query)
        {
        }

        protected override Task<User> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId);
        }
    }
}
