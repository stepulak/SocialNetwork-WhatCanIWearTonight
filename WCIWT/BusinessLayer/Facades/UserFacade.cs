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
        private readonly IUserService userService;
        private readonly IFriendshipService friendshipService;

        public UserFacade(IUnitOfWorkProvider unitOfWorkProvider, IUserService userService, 
            IFriendshipService friendshipService) 
            : base(unitOfWorkProvider)
        {
            this.userService = userService;
            this.friendshipService = friendshipService;
        }
        
        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await userService.GetUserByUsernameAsync(username);
            }
        }

        public async Task<QueryResultDto<UserDto, UserFilterDto>> GetAllUsersAsync(UserFilterDto filter = null)
        {
            using (UnitOfWorkProvider.Create())
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

        public async Task<Guid> RegisterUser(UserCreateDto userCreate)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                try
                {
                    var id = await userService.RegisterUserAsync(userCreate);
                    await uow.Commit();
                    return id;
                }
                catch (ArgumentException)
                {
                    throw;
                }
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await userService.AuthorizeUserAsync(username, password);
            }
        }

        public async Task<bool> IsAdmin(string username)
        {
            using (UnitOfWorkProvider.Create())
            {
                var users = await userService.ListUsersAsync(new UserFilterDto { Username = username });
                return users.TotalItemsCount == 1 && users.Items.First().IsAdmin;
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
            using (UnitOfWorkProvider.Create())
            {
                var filter = new FriendshipFilterDto
                {
                    UserA = userId,
                    IsConfirmed = false
                };
                var friendships = await friendshipService.ListFriendshipAsync(filter);
                friendships.Items = friendships.Items.Where(friendship => friendship.RecipientId == userId);
                return friendships;
            }
        }

        public async Task<bool> CanSendFriendshipRequest(UserDto applicant, UserDto recipient)
        {
            using (UnitOfWorkProvider.Create())
            {
                var filter = new FriendshipFilterDto
                {
                    // We can swap those, doesn't really matter the order
                    UserA = applicant.Id,
                    UserB = recipient.Id
                };
                var allFriendships = await friendshipService.ListFriendshipAsync(filter);
                filter.IsConfirmed = false;
                var allFriendshipRequests = await friendshipService.ListFriendshipAsync(filter);
                return allFriendships.TotalItemsCount == 0 && allFriendshipRequests.TotalItemsCount == 0;
            }
        }

        public async Task<Guid> SendFriendshipRequest(UserDto applicant, UserDto recipient)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (await CanSendFriendshipRequest(applicant, recipient) == false)
                {
                    throw new InvalidOperationException($"Unable to send friendship request {applicant} -> {recipient}. Already exists.");
                }
                var id = friendshipService.Create(new FriendshipDto
                {
                    ApplicantId = applicant.Id,
                    RecipientId = recipient.Id,
                    IsConfirmed = false
                });
                await uow.Commit();
                return id;
            }

        }

        public async Task ConfirmFriendshipRequest(FriendshipDto friendship)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                friendship.IsConfirmed = true;
                await friendshipService.Update(friendship);
                await uow.Commit();
            }
        }

        public async Task CancelFriendshipRequest(FriendshipDto friendship)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                friendshipService.Delete(friendship.Id);
                await uow.Commit();
            }
        }

        public async Task RemoveFriendship(FriendshipDto friendship)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                friendshipService.Delete(friendship.Id);
                await uow.Commit();
            }
        }

        public async Task<QueryResultDto<UserDto, FriendshipFilterDto>> GetFriendsOfUser(Guid userId, FriendshipFilterDto filter)
        {
            filter.UserA = userId;
            filter.IsConfirmed = true;
            using (UnitOfWorkProvider.Create())
            {
                return await friendshipService.GetFriendsOfUserAsync(userId, filter);
            }
        }

        public async Task<FriendshipDto> GetFriendshipBetweenUsers(Guid userAId, Guid userBId)
        {
            var filter = new FriendshipFilterDto
            {
                UserA = userAId,
                UserB = userBId
            };

            using (UnitOfWorkProvider.Create())
            {
                var result = await friendshipService.ListFriendshipAsync(filter);
                return result.Items.FirstOrDefault();
            }
        }

        public async Task<FriendshipDto> GetFriendshipRequestBetweenUsers(Guid userAId, Guid userBId)
        {
            var filter = new FriendshipFilterDto
            {
                UserA = userAId,
                UserB = userBId,
                IsConfirmed = false
            };
            using (UnitOfWorkProvider.Create())
            {
                var result = await friendshipService.ListFriendshipAsync(filter);
                return result.Items.FirstOrDefault();
            }
        }

        public async Task RemoveUser(Guid userId)
        {
            await RemoveFriendshipsForUser(userId);
            using (var uow = UnitOfWorkProvider.Create())
            {
                userService.Delete(userId);
                await uow.Commit();
            }
        }

        private async Task RemoveFriendshipsForUser(Guid userId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var friendships = await friendshipService.ListFriendshipAsync(new FriendshipFilterDto { UserA = userId });
                foreach (var friendship in friendships.Items)
                {
                    friendshipService.Delete(friendship.Id);
                }
                await uow.Commit();
            }
        }
    }
}
