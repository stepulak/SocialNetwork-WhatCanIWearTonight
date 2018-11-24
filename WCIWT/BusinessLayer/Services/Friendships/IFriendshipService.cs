using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Friendships
{
    public interface IFriendshipService
    {
        Task<QueryResultDto<FriendshipDto, FriendshipFilterDto>> ListFriendshipAsync(FriendshipFilterDto filter);
    }
}
