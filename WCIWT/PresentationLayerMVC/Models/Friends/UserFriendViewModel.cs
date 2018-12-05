using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLayer.DataTransferObjects;

namespace PresentationLayerMVC.Models.Friends
{
    public class UserFriendViewModel
    {
        public UserDto User;
        public bool IsFriendWithLoggedUser;
    }
}