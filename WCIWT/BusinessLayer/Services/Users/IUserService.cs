using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Users
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(Guid id, bool withIncludes = true);
        Guid Create(UserDto entityDto);
        Task Update(UserDto entityDto);
        void Delete(Guid entityId);
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<QueryResultDto<UserDto, UserFilterDto>> ListAllAsync();
        Task<QueryResultDto<UserDto, UserFilterDto>> ListUsersAsync(UserFilterDto filter);
        Task<Guid> RegisterUserAsync(UserCreateDto user);
        Task<bool> AuthorizeUserAsync(string username, string password);
    }
}
