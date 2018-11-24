using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using EntityDatabase;

namespace BusinessLayer.Services.PostServices
{
    public interface IHashtagService
    {
        Task<QueryResultDto<HashtagDto, HashtagFilterDto>> ListHashtagAsync(HashtagFilterDto filter);
    }
}