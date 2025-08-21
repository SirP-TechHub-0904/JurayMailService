using Azure;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostmarkEmailService;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.Repositories
{
    public sealed class EmailSendingStatusRepository : Repository<EmailSendingStatus>, IEmailSendingStatusRepository
    {
        private readonly AppDBContext _context;
        private readonly PostmarkClient _postmarkClient;
        private readonly IConfiguration _configuration;

        public EmailSendingStatusRepository(AppDBContext context, PostmarkClient postmarkClient, IConfiguration configuration) : base(context)
        {
            _context = context;
            _postmarkClient = new PostmarkClient(GetServerTokenFromSettings());
            _configuration = configuration;
        }
        private string GetServerTokenFromSettings()
        {
            return "cbd0e0f4-d49d-4f7e-9412-9e741a8f8705";
            //return _configuration.GetSection("PostmarkSettings")["ServerToken"];
        }
        public async Task<RegisterGroupEmailsDto> AddEmailsForSending(long projectId, long? groupId, bool sendToAllGroup, string bulkUserInput)
        {


            RegisterGroupEmailsDto result = new RegisterGroupEmailsDto();


            try
            {
                var getProject = await _context.EmailProjects.FindAsync(projectId);

                if (getProject != null)
                {
                    var recipients = new List<EmailListDto>();

                    // 1) Bulk manual entries
                    if (!string.IsNullOrWhiteSpace(bulkUserInput))
                    {
                        // Split entries by comma OR newline
                        var entries = Regex.Split(bulkUserInput ?? "", @"[,\r\n]+")
                                           .Where(s => !string.IsNullOrWhiteSpace(s));

                        var bulkDtos = entries
                            .Select(e => e.Split(':', 3, StringSplitOptions.RemoveEmptyEntries)) // allow 2 or 3 fields
                            .Where(parts => parts.Length >= 2) // at least Name + Email
                            .Select(parts => new EmailListDto
                            {
                                Name = parts[0].Trim(),
                                Email = parts[1].Trim(),
                                PhoneNumber = parts.Length >= 3 ? parts[2].Trim() : null
                            })
                            .Where(d => !string.IsNullOrWhiteSpace(d.Email))
                            .ToList();

                        if (bulkDtos.Count > 0)
                        {
                            recipients.AddRange(bulkDtos);
                        }
                    }

                    // 2) All groups OR a single group
                    if (sendToAllGroup)
                    {
                        var allDtos = await _context.EmailLists
                            .AsNoTracking()
                            .Where(x => x.AppUserId == getProject.AppUserId)
                            .Select(x => new EmailListDto
                            {
                                Email = x.Email,
                                Name = x.Name,
                                PhoneNumber = x.PhoneNumber
                            })
                            .ToListAsync();

                        if (allDtos.Count > 0)
                        {
                            recipients.AddRange(allDtos);
                        }
                    }
                    else
                    {
                        var group = await _context.EmailGroups.FindAsync(groupId);
                        if (group != null)
                        {
                            var groupDtos = await _context.EmailLists
                                .AsNoTracking()
                                .Where(x => x.EmailGroupId == groupId)
                                .Select(x => new EmailListDto
                                {
                                    Email = x.Email,
                                    Name = x.Name,
                                    PhoneNumber = x.PhoneNumber
                                })
                                .ToListAsync();

                            if (groupDtos.Count > 0)
                            {
                                recipients.AddRange(groupDtos);
                            }
                        }
                    }

                    // 3) De-duplicate by Email (avoid double submits if sources overlap)
                    recipients = recipients
                        .Where(r => !string.IsNullOrWhiteSpace(r.Email))
                         .DistinctBy(r => r.Email.Trim().ToLowerInvariant())
                        .ToList();

                    // 4) Single submit + single result update
                    await AddEmailSendingStatuses(recipients, getProject.Id, getProject.AppUserId);

                    result.Submitted = recipients.Count;
                    result.Success = recipients.Count > 0;

                }

            }
            catch (Exception ex)
            {
                result.Success = false;
            }

            return result;
        }

        public async Task<IEnumerable<EmailSendingStatus>> GetListByUserIdAsync(int pageNumber, int pageSize, string userId, long? groupSendingProjectId)
        {
            var query = _context.EmailSendingStatuses
        .Include(x => x.GroupSendingProject)
        .Where(x => x.UserId == userId);

            // apply filter only if groupSendingProjectId is supplied
            if (groupSendingProjectId.HasValue && groupSendingProjectId.Value > 0)
            {
                query = query.Where(x => x.GroupSendingProjectId == groupSendingProjectId.Value);
            }

            var list = await query
                .OrderByDescending(e => e.SubmittedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return list;
        }

        public async Task<int> GetTotalCountByUserIdAsync(string userId, long? projectId)
        {
            var query = _context.EmailSendingStatuses
                    .Where(x => x.UserId == userId);

            if (projectId.HasValue && projectId.Value > 0)
            {
                query = query.Where(x => x.EmailProjectId == projectId.Value);
            }

            return await query.CountAsync();

             
        }

        public async Task<List<EmailSendingStatus>> ListByUserId(string userId)
        {
            var list = await _context.EmailSendingStatuses.Where(x => x.UserId == userId).ToListAsync();
            return list;
        }

        public async Task SendBatchEmailByEmailIds()
        {
            var getemails = await _context.EmailSendingStatuses
                .AsNoTracking()
                .Include(x => x.EmailProject)
                .Where(x => x.SendingStatus == Domain.Enum.EnumStatus.SendingStatus.Pending &&
            x.Retries <= 5).Take(30).ToListAsync();

            foreach (var ms in getemails)
            {
                //get sender email and name
                var server = await _context.Servers.FirstOrDefaultAsync(x => x.UserId == ms.UserId && x.Disable == false);
                if (server == null)
                {
                    continue;
                }

                // Replace {{...}} placeholders (add Date if you like)
                string inner = ms.EmailProject.Template
                    .Replace("{{Name}}", ms.Name ?? string.Empty)
                    .Replace("{{Email}}", ms.Email ?? string.Empty)
                    .Replace("{{PhoneNumber}}", ms.PhoneNumber ?? string.Empty)
                    .Replace("{{Date}}", DateTime.UtcNow.AddHours(1).ToString("MMMM d, yyyy")); // optional

                // Minimal, email-safe HTML wrapper
                string emailbody = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""utf-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
  <title>{System.Net.WebUtility.HtmlEncode(ms.EmailProject.Subject ?? "Newsletter")}</title>
  <style>
    /* keep styles simple for email clients */
    body {{ margin:0; padding:0; background:#f6f6f6; -webkit-text-size-adjust:100%; }}
    .wrapper {{ width:100%; background:#f6f6f6; padding:24px 0; }}
    .container {{ max-width:600px; margin:0 auto; background:#ffffff; padding:24px; font-family:Arial, Helvetica, sans-serif; color:#222; line-height:1.5; }}
    a {{ color:#0b5ed7; text-decoration:none; }}
    p {{ margin:0 0 12px 0; }}
  </style>
</head>
<body>
  <div class=""wrapper"">
    <div class=""container"">
      {inner}
    </div>
  </div>
</body>
</html>";



                var updateSenderMail = await _context.EmailSendingStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == ms.Id);
                try
                {

                    var message = new PostmarkMessage
                    {
                        From = $"{server.SenderName} <{server.SenderEmail}>",
                        To = ms.Email,
                        Subject = ms.EmailProject.Subject,
                        HtmlBody = emailbody
                    };

                    PostmarkResponse response = await _postmarkClient.SendMessageAsync(message);

                    if (response.Status == 0)
                    {
                        updateSenderMail.SendingStatus = Domain.Enum.EnumStatus.SendingStatus.Sent;
                        updateSenderMail.SentDate = DateTime.UtcNow.AddHours(1);
                        updateSenderMail.Log = response.Message + $"({response.MessageID})";
                        updateSenderMail.MessageId = response.MessageID.ToString();

                        _context.Entry(updateSenderMail).State = EntityState.Modified;
                    }
                    else
                    {
                        updateSenderMail.SendingStatus = Domain.Enum.EnumStatus.SendingStatus.Failed;
                        updateSenderMail.Retries += 1;
                        updateSenderMail.Log = response.Message + $"({response.MessageID})";
                        updateSenderMail.MessageId = response.MessageID.ToString();

                        _context.Entry(updateSenderMail).State = EntityState.Modified;
                    }

                }
                catch (Exception ex)
                {
                    updateSenderMail.SendingStatus = Domain.Enum.EnumStatus.SendingStatus.Failed;
                    updateSenderMail.Retries += 1;
                    updateSenderMail.Log = ex.ToString();

                    _context.Entry(updateSenderMail).State = EntityState.Modified;
                }
            }
            await _context.SaveChangesAsync();

        }

        private async Task AddEmailSendingStatuses(IEnumerable<EmailListDto> emails, long projectId, string userId)
        {
            var distinctEmails = emails.Select(x => x.Email).Distinct();
            int count = 0;

            GroupSendingProject groupSendingProject = new GroupSendingProject();
            groupSendingProject.EmailProjectId = projectId;

            groupSendingProject.Date = DateTime.UtcNow.AddHours(1);
            await _context.GroupSendingProjects.AddAsync(groupSendingProject);
            await _context.SaveChangesAsync();
            foreach (var email in distinctEmails)
            {
                EmailSendingStatus sendmail = new EmailSendingStatus
                {
                    SendingStatus = Domain.Enum.EnumStatus.SendingStatus.Pending,
                    Retries = 0,
                    Email = email,
                    EmailProjectId = projectId,
                    UserId = userId,
                    SubmittedDate = DateTime.UtcNow.AddHours(1),
                    GroupSendingProjectId = groupSendingProject.Id
                };
                count++;
                await _context.EmailSendingStatuses.AddAsync(sendmail);
            }
            // Update the submitted count AFTER adding statuses
            groupSendingProject.Submitted = distinctEmails.Count();
            _context.GroupSendingProjects.Update(groupSendingProject);
            await _context.SaveChangesAsync();
        }

        public async Task<GetWebHookUpdateIds> GetEmailListIdByMessageId(string messageId)
        {
            GetWebHookUpdateIds result = new GetWebHookUpdateIds();
            var data = await _context.EmailSendingStatuses.FirstOrDefaultAsync(x => x.MessageId == messageId);
            result.EmailId = 0;
            result.UserId = data.UserId;
            result.EmailProjectId = data.EmailProjectId;
            return result;
        }

        public async Task<EmailSendingStatus> GetEmailSendingById(long emailId)
        {
            var data = await _context.EmailSendingStatuses
                 .Include(x => x.EmailProject)
                 .FirstOrDefaultAsync(x => x.Id == emailId);

            return data;
        }

        public async Task<IEnumerable<EmailResponseStatus>> GetResponseListByUserIdAsync(int pageNumber, int pageSize, string userId, long? projectId)
        {
            // Base query for this user
            var query = _context.EmailResponseStatuses
                .Where(x => x.UserId == userId);

            // Only filter by project if provided
            if (projectId.HasValue && projectId.Value > 0)
            {
                query = query.Where(x => x.EmailProjectId == projectId.Value);
            }

            // Paging
            var list = await query
                .OrderByDescending(e => e.SentDate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return list;
        }

        public async Task<int> GetResponseTotalCountByUserIdAsync(string userId, long? projectId)
        {
            var query = _context.EmailResponseStatuses
                    .Where(x => x.UserId == userId);

            if (projectId.HasValue && projectId.Value > 0)
            {
                query = query.Where(x => x.EmailProjectId == projectId.Value);
            }

            return await query.CountAsync();
        }
    }
}
