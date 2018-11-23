using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using X.PagedList;

namespace PresentationLayerMVC.Models.Posts
{
    public class PostListViewModel
    {
        public IPagedList<PostDto> Posts { get; set; }
        public PostFilterDto Filter { get; set; }
    }
}