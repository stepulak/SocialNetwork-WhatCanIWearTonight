using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
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

        Task<Guid> RegisterUserAsync(UserCreateDto user);

        Task<bool> AuthorizeUserAsync(string username, string password);
    }
}
