using System.Collections;
using System.Collections.Generic;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;

namespace PresentationLayerMVC.Models.Friends
{
    public class FriendsListViewModel
    {
        public IList<UserDto> Friends { get; set; }
        public UserFilterDto Filter { get; set; }
    }
}