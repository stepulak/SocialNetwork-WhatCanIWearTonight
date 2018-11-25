using System.Collections.Generic;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using X.PagedList;

namespace PresentationLayerMVC.Models.Posts
{
    public class PostWithReplysViewModel
    {
        public PostDto Post { get; set; }
        public IPagedList<ImageDto> Images { get; set; }
        public IPagedList<PostReplyDto> Replys { get; set; }
        public PostFilterDto PostFilter { get; set; }
        public PostReplyFilterDto PostReplyFilter { get; set; }
        public ImageFilterDto ImageFilter { get; set; }
    }
}