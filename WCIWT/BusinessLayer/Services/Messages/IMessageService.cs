using System;
using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Messages
{
    public interface IMessageService
    {
        Task<MessageDto> GetAsync(Guid id, bool withIncludes = true);
        Guid Create(MessageDto entityDto);
        Task Update(MessageDto entityDto);
        void Delete(Guid entityId);
        Task<QueryResultDto<MessageDto, MessageFilterDto>> ListAllAsync();
        Task<QueryResultDto<MessageDto, MessageFilterDto>> ListMessageAsync(MessageFilterDto filter);
    }
}
