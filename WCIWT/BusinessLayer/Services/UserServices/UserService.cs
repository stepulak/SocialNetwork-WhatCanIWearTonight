using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.DataTransferObjects.Filters.Common;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System;
using System.Threading.Tasks;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Services.UserServices
{
    public class UserService : CrudQueryServiceBase<User, UserDto, UserFilterDto>, IUserService
    {
        private readonly QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>> userQueryObject;


        public UserService(IMapper mapper, IRepository<User> repository, UserQueryObject query,
             QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>> userQueryObject)
            : base(mapper, repository, query)
        {
            this.userQueryObject = userQueryObject;
        }

        public async Task<QueryResultDto<UserDto, UserFilterDto>> ListUsersAsync(UserFilterDto filter)
        {
            return await userQueryObject.ExecuteQuery(filter);
        }

        protected override Task<User> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId);
        }
    }
}
