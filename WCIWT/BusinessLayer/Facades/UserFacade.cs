using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.UnitOfWork;

namespace BusinessLayer.Facades.Common
{
    public class UserFacade : FacadeBase
    {
        private readonly IUserService userService;

        public UserFacade(IUnitOfWorkProvider unitOfWorkProvider, IUserService userService) : base(unitOfWorkProvider)
        {
            this.userService = userService;
        }

        public async Task<QueryResultDto<UserDto, UserFilterDto>> GetAllUsersAsync(UserFilterDto filter = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await userService.ListUsersAsync(filter);
            }
        }

        public Guid RegisterUser(UserDto userRegistration)
        {
            UserFilterDto UsernameAndEmailFilter = new UserFilterDto()
            {
                Username = userRegistration.Username,
                Email = userRegistration.Email
            };

            if (GetAllUsersAsync(UsernameAndEmailFilter).Result.TotalItemsCount != 0)
            {
                // throw error here
            }

            return userService.Create(userRegistration);
        }

        public void UnregisterUser(UserDto user)
        {

        }
        
        public bool AddFriendshipRequest()
        {
            throw new NotImplementedException();
        }

        public bool ConfirmFriendshipRequest()
        {
            throw new NotImplementedException();
        }
    }
}
