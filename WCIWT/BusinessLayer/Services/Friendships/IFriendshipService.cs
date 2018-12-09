using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Friendships
{
    public interface IFriendshipService
    {
        Task<FriendshipDto> GetAsync(Guid id, bool withIncludes = true);
        Guid Create(FriendshipDto entityDto);
        Task Update(FriendshipDto entityDto);
        void Delete(Guid entityId);
        Task<QueryResultDto<FriendshipDto, FriendshipFilterDto>> ListAllAsync();
        Task<QueryResultDto<FriendshipDto, FriendshipFilterDto>> ListFriendshipAsync(FriendshipFilterDto filter);
        Task<List<UserDto>> ListOfFriendsAsync(Guid userId);
        Task<QueryResultDto<UserDto, FriendshipFilterDto>> GetFriendsOfUserAsync(Guid userId,
            FriendshipFilterDto filter);
        Task<List<UserDto>> ListOfFriendRequestsAsync(Guid userId);
    }
}
