using System.Collections;
using System.Collections.Generic;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using X.PagedList;

namespace PresentationLayerMVC.Models.Friends
{
    public class FriendsListViewModel
    {
        public IPagedList<UserDto> Friends { get; set; }
        public UserFilterDto Filter { get; set; }
    }
}