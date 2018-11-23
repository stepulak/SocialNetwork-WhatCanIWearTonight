using System.Collections.Generic;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using X.PagedList;

namespace PresentationLayerMVC.Models.FriendRequests
{
    public class FriendRequestListViewModel
    {
        public IPagedList<FriendshipDto> FriendRequests { get; set; }
        public FriendshipFilterDto Filter { get; set; }
    }
    
}