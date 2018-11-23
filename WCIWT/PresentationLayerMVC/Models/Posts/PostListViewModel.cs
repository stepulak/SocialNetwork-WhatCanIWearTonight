using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;

namespace PresentationLayerMVC.Models.Posts
{
    public class PostListViewModel
    {
        public IList<PostDto> Posts { get; set; }
        public PostFilterDto Filter { get; set; }
    }
}