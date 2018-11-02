
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using EntityDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.UserServices
{
    public interface IMessageService
    {
        Task<QueryResultDto<MessageDto, MessageFilterDto>> ListMessageAsync(MessageFilterDto filter);
    }
}
