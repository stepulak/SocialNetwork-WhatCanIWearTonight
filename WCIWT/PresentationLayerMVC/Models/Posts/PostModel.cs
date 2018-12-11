using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Filters;
using X.PagedList;

namespace PresentationLayerMVC.Models.Posts
{
    public class PostModel
    {
        // TODO: in/out separate classes

        // Out
        public PostDto Post { get; set; }
        public IPagedList<ImageDto> Images { get; set; }
        public IPagedList<PostReplyDto> Replys { get; set; }
        public PostFilterDto PostFilter { get; set; }
        public PostReplyFilterDto PostReplyFilter { get; set; }
        public ImageFilterDto ImageFilter { get; set; }
        public List<Tuple<int, int>> HashtagIndices { get; set; }

        // In
        // Comment
        public string PostId { get; set; }
        public string Username { get; set; }
        public string TextComment { get; set; }
    }
}