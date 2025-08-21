using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GroupSendingProjectQueries
{
    public sealed class GetByIdGroupSendingProjectQuery : IRequest<GroupSendingProject>
    {
        public GetByIdGroupSendingProjectQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

        public class GetByIdGroupSendingProjectQueryHandler : IRequestHandler<GetByIdGroupSendingProjectQuery, GroupSendingProject>
        {
            private readonly IGroupSendingProjectRepository _groupSendingProjectRepository;

            public GetByIdGroupSendingProjectQueryHandler(IGroupSendingProjectRepository groupSendingProjectRepository)
            {
                _groupSendingProjectRepository = groupSendingProjectRepository;
            }

            public async Task<GroupSendingProject> Handle(GetByIdGroupSendingProjectQuery request, CancellationToken cancellationToken)
            {
                return await _groupSendingProjectRepository.GetById(request.Id);

            }
        }
    }

}
