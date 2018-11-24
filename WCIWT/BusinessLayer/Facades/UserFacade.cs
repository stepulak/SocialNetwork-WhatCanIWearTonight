using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Facades.Common;
using BusinessLayer.Services.Friendships;
using BusinessLayer.Services.Users;
using WCIWT.Infrastructure.UnitOfWork;

namespace BusinessLayer.Facades
{
    public class UserFacade : FacadeBase
    {
        private readonly UserService userService;
        private readonly FriendshipService friendshipService;

        public UserFacade(IUnitOfWorkProvider unitOfWorkProvider, UserService userService, 
            FriendshipService friendshipService) 
            : base(unitOfWorkProvider)
        {
            this.userService = userService;
            this.friendshipService = friendshipService;
        }

        public async Task<QueryResultDto<UserDto, UserFilterDto>> GetAllUsersAsync(UserFilterDto filter = null)
        {
            using (UnitOfWorkProvider.Create()) // ???
            {
                return await userService.ListUsersAsync(filter);
            }
        }

        public async Task<UserDto> GetUserAsync(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await userService.GetAsync(id);
            }
        }

        public async Task<Guid> RegisterUser(UserDto user)
        {
            var filter = new UserFilterDto
            {
                Username = user.Username,
                Email = user.Email
            };
            var allUsers = await GetAllUsersAsync(filter);
            if (allUsers.TotalItemsCount != 0)
            {
                throw new InvalidOperationException($"Unable to register new user {user}. Already exists.");
            }
            return userService.Create(user);
        }

        public async Task<bool> UnregisterUser(UserDto user)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var filter = new UserFilterDto
                {
                    Username = user.Username,
                    Email = user.Email
                };
                var allUsers = await GetAllUsersAsync(filter);
                if (allUsers.TotalItemsCount == 0)
                {
                    return false;
                }
                userService.Delete(user.Id);
                await uow.Commit();
                return true;
            }
        }

        public async Task<bool> EditUserAsync(UserDto userDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (await userService.GetAsync(userDto.Id, false) == null)
                {
                    return false;
                }

                await userService.Update(userDto);
                await uow.Commit();
                return true;
            }
        }
        
        public async Task<QueryResultDto<FriendshipDto, FriendshipFilterDto>> PendingFriendshipRequests(Guid userId)
        {
            var filter = new FriendshipFilterDto
            {
                UserA = userId
            };
            return await friendshipService.ListFriendshipAsync(filter);
        }

        public async Task<bool> CanSendFrienshipRequest(UserDto applicant, UserDto recipient)
        {
            var filter = new FriendshipFilterDto
            {
                // We can swap those, doesn't really matter the order
                UserA = applicant.Id,
                UserB = recipient.Id
            };
            var allFriendships = await friendshipService.ListFriendshipAsync(filter);
            return allFriendships.TotalItemsCount == 0;
        }

        public async Task SendFriendshipRequest(UserDto applicant, UserDto recipient)
        {
            if (await CanSendFrienshipRequest(applicant, recipient) == false)
            {
                throw new InvalidOperationException($"Unable to send friendship request {applicant} -> {recipient}. Already exists.");
            }
            friendshipService.Create(new FriendshipDto
            {
                ApplicantId = applicant.Id,
                RecipientId = recipient.Id,
                IsConfirmed = false
            });
        }

        public async Task ConfirmFriendshipRequest(FriendshipDto frienship)
        {
            frienship.IsConfirmed = true;
            await friendshipService.Update(frienship);
        }

        public void CancelFriendshipRequest(FriendshipDto friendship) 
            => friendshipService.Delete(friendship.Id);

        public async Task<List<UserDto>> GetFriendsOfUser(Guid userId)
        {
            return await friendshipService.ListOfFriendsAsync(userId);
        }

    }
}
