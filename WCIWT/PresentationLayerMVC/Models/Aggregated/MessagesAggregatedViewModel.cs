using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int Page { get; set; }

        [Required(ErrorMessage = "Message must not be empty")]
        public string NewMessageText { get; set; }
    }
}