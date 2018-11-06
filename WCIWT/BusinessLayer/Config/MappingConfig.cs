using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using EntityDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure.Query;

namespace BusinessLayer.Config
{
    public class MappingConfig
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<Hashtag, HashtagDto>();
            config.CreateMap<HashtagDto, Hashtag>();

            config.CreateMap<Friendship, FriendshipDto>();
            config.CreateMap<FriendshipDto, Friendship>();

            config.CreateMap<Message, MessageDto>();
            config.CreateMap<MessageDto, Message>();

            config.CreateMap<Post, PostDto>();
            config.CreateMap<PostDto, Post>();

            config.CreateMap<PostReply, PostReplyDto>();
            config.CreateMap<PostReplyDto, PostReply>();

            config.CreateMap<User, UserDto>();
            config.CreateMap<UserDto, User>();

            config.CreateMap<Vote, VoteDto>();
            config.CreateMap<VoteDto, Vote>();

            config.CreateMap<QueryResult<Friendship>, QueryResultDto<FriendshipDto, FriendshipFilterDto>>();
            config.CreateMap<QueryResult<Hashtag>, QueryResultDto<HashtagDto, HashtagFilterDto>>();
            config.CreateMap<QueryResult<Image>, QueryResultDto<ImageDto, ImageFilterDto>>();
            config.CreateMap<QueryResult<Message>, QueryResultDto<MessageDto, MessageFilterDto>>();
            config.CreateMap<QueryResult<Post>, QueryResultDto<PostDto, PostFilterDto>>();
            config.CreateMap<QueryResult<PostReply>, QueryResultDto<PostReplyDto, PostReplyFilterDto>>();
            config.CreateMap<QueryResult<User>, QueryResultDto<UserDto, UserFilterDto>>();
            config.CreateMap<QueryResult<Vote>, QueryResultDto<VoteDto, VoteFilterDto>>();
        }
    }
}
