using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.UserServices
{
    public interface IUserService
    {
        Task<QueryResultDto<UserDto, UserFilterDto>> ListUsersAsync(UserFilterDto filter);
    }
}
