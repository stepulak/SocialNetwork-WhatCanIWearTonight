using System.Threading.Tasks;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;

namespace BusinessLayer.Services.Messages
{
    public interface IMessageService
    {
        Task<QueryResultDto<MessageDto, MessageFilterDto>> ListMessageAsync(MessageFilterDto filter);
    }
}
