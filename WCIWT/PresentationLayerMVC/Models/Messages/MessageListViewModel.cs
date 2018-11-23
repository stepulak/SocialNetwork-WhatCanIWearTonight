using System.Collections;
using System.Collections.Generic;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;

namespace PresentationLayerMVC.Models.Messages
{
    public class MessageListViewModel
    {
        public IList<MessageDto> Messages { get; set; }
        public MessageFilterDto Filter { get; set; }
    }
}