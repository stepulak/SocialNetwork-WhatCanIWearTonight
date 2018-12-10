using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BusinessLayer.DataTransferObjects;

namespace PresentationLayerMVC.Models.Posts
{
    public class CreatePostModel
    {
        public PostCreateDto Post { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }

        public CreatePostModel()
        {
            Files = new List<HttpPostedFileBase>();
        }
    }
}