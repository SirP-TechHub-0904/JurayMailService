using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.EmailSendingStatusCommands
{
    public sealed class AddEmailSendingStatusCommand : IRequest<RegisterGroupEmailsDto>
    {
        public AddEmailSendingStatusCommand(long projectId, long? groupId, bool sendToAllGroup, string bulkUserInput)
        {
            ProjectId = projectId;
            GroupId = groupId;
            SendToAllGroup = sendToAllGroup;
            BulkUserInput = bulkUserInput;
        }

        public long ProjectId { get; set; }
        public long? GroupId { get; set; }
        public bool SendToAllGroup { get; set; }
        public string BulkUserInput { get; set; }
    }

    public class AddEmailSendingStatusCommandHandler : IRequestHandler<AddEmailSendingStatusCommand, RegisterGroupEmailsDto>
    {
        private readonly IEmailSendingStatusRepository _emailSendingStatusRepository;

        public AddEmailSendingStatusCommandHandler(IEmailSendingStatusRepository emailSendingStatusRepository)
        {
            _emailSendingStatusRepository = emailSendingStatusRepository;
        }

        public async Task<RegisterGroupEmailsDto> Handle(AddEmailSendingStatusCommand request, CancellationToken cancellationToken)
        {

            var result = await _emailSendingStatusRepository.AddEmailsForSending(request.ProjectId, request.GroupId, request.SendToAllGroup, request.BulkUserInput);

            return result;
        }
    }

    
}
