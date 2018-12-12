using System;
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
        public List<List<ImageDto>> ImagesForPosts { get; set; }
        public List<List<Tuple<int, int>>> HashtagIndices { get; set; }
        public PostFilterDto PostFilter { get; set; }
    }
}