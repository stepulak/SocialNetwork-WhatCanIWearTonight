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

        public async Task<Guid> RegisterUser(UserDto user)
        {
            var filter = new UserFilterDto()
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

        public async Task UnregisterUser(UserDto user)
        {
            var filter = new UserFilterDto()
            {
                Username = user.Username,
                Email = user.Email
            };
            var allUsers = await GetAllUsersAsync(filter);
            if (allUsers.TotalItemsCount == 0)
            {
                throw new InvalidOperationException($"Unable to unregister user {user}. Not found.");
            }
            userService.Delete(user.Id);
        }
        
        public async Task<List<FriendshipDto>> PendingFriendshipRequests(UserDto user)
        {
            var filter = new FriendshipFilterDto()
            {
                UserA = user.Id
            };
            var result = await friendshipService.ListFriendshipAsync(filter);
            return result.Items.ToList();
        }

        public async Task<bool> CanSendFrienshipRequest(UserDto applicant, UserDto recipient)
        {
            var filter = new FriendshipFilterDto()
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
    }
}
