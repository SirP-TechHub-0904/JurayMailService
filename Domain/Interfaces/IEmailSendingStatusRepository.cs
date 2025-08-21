using Domain.DTO;
using Domain.GenericInterface;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmailSendingStatusRepository : IRepository<EmailSendingStatus>
    {
        Task<RegisterGroupEmailsDto> AddEmailsForSending(long projectId, long? groupId, bool sendToAllGroup, string bulkUserInput);
        Task<List<EmailSendingStatus>> ListByUserId(string userId);

        Task<IEnumerable<EmailSendingStatus>> GetListByUserIdAsync(int pageNumber, int pageSize, string userId, long? groupSendingProjectId);
        Task<IEnumerable<EmailResponseStatus>> GetResponseListByUserIdAsync(int pageNumber, int pageSize, string userId, long? projectId);

        Task<int> GetTotalCountByUserIdAsync(string userId, long? projectId);
        Task<int> GetResponseTotalCountByUserIdAsync(string userId, long? projectId);

        Task SendBatchEmailByEmailIds();

        Task<GetWebHookUpdateIds> GetEmailListIdByMessageId(string  messageId);

        Task<EmailSendingStatus> GetEmailSendingById(long emailId);
    }
}
