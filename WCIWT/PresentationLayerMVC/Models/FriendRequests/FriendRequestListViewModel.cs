using System.Collections.Generic;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;

namespace PresentationLayerMVC.Models.FriendRequests
{
    public class FriendRequestListViewModel
    {
        public IList<FriendshipDto> FriendRequests { get; set; }
        public FriendshipFilterDto Filter { get; set; }
    }
    
}