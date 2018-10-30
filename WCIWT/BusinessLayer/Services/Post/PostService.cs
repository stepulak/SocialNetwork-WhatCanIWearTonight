using AutoMapper;
using BusinessLayer.Services.Common;
using WCIWT.Infrastructure;
using WCIWT.
using EntityDatabase;

namespace BusinessLayer.Services.Post
{
    public class PostService : ServiceBase, IPostService
    {
        public PostService(IMapper mapper, IRepository<User> postRepository,) : base(mapper)
        {
        }
        
    }
}
