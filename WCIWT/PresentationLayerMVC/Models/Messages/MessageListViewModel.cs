using System.Collections;
using System.Collections.Generic;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using X.PagedList;

namespace PresentationLayerMVC.Models.Messages
{
    public class MessageListViewModel
    {
        public IPagedList<MessageDto> Messages { get; set; }
        public MessageFilterDto Filter { get; set; }
    }
}