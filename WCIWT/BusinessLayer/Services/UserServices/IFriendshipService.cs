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
    public interface IFriendshipService
    {
        Task<QueryResultDto<FriendshipDto, FriendshipFilterDto>> ListFriendshipAsync(FriendshipFilterDto filter);
    }
}
