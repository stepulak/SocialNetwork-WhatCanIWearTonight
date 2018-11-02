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
        Task<UserDto> GetAsync(Guid id, bool withIncludes = true);

        Guid Create(UserDto userDto);

        Task Update(UserDto userDto);

        Task<QueryResultDto<UserDto, UserFilterDto>> ListUsersAsync(UserFilterDto filter);
    }
}
