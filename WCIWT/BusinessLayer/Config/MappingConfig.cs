using AutoMapper;
using BusinessLayer.DataTransferObjects;
using EntityDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Config
{
    public class MappingConfig
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<Hashtag, HashtagDto>();
            config.CreateMap<HashtagDto, Hashtag>().ForMember(dest => dest.HashtagInPosts, opt => opt.Ignore());

            config.CreateMap<Friendship, FriendshipDto>();
            config.CreateMap<FriendshipDto, Friendship>();

            // TODO: Create mapping for other entities
        }
    }
}
