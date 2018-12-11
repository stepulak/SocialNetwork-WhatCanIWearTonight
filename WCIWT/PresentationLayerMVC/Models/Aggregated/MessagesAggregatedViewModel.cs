using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLayer.DataTransferObjects;
using PresentationLayerMVC.Models.Messages;

namespace PresentationLayerMVC.Models.Aggregated
{
    public class MessagesAggregatedViewModel
    {
        public MessageListViewModel MessagesListView { get; set; }
        public UserDto Friend { get; set; }

        public string NewMessageText { get; set; }
    }
}