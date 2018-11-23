using System.Collections.Generic;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;

namespace PresentationLayerMVC.Models.Posts
{
    public class PostWithReplysViewModel
    {
        public PostDto Post { get; set; }
        public IList<PostReplyDto> Replys { get; set; }
        public PostFilterDto PostFilter { get; set; }
        public PostReplyFilterDto PostReplyFilter { get; set; }
    }
}