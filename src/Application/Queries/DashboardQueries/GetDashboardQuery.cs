using Application.DTO;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.DashboardQueries
{
    public sealed class GetDashboardQuery : IRequest<DashboardDto>
    {
        public GetDashboardQuery(string? userId)
        {
            UserId = userId;
        }

        public string? UserId { get; set; }


        public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
        {

            private readonly IEmailResponseStatusRepository _emailresponse;
            private readonly IEmailListRepository _emailListRepository;
            private readonly IEmailProjectRepository _emailProjectRepository;

            private readonly IWalletRepository _walletRepository;
            private readonly IAccountSubscriptionRepository _userSubscriptionRepository;

            public GetDashboardQueryHandler(IEmailListRepository emailListRepository, IEmailProjectRepository emailProjectRepository, IEmailResponseStatusRepository emailresponse, IWalletRepository walletRepository, IAccountSubscriptionRepository userSubscriptionRepository)
            {
                _emailListRepository = emailListRepository;
                _emailProjectRepository = emailProjectRepository;
                _emailresponse = emailresponse;
                _walletRepository = walletRepository;
                _userSubscriptionRepository = userSubscriptionRepository;
            }

            public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
            {
                DashboardDto dashboard = new DashboardDto();

                //get project
                dashboard.AllProjects = await _emailProjectRepository.GetProjectCountByUserId(request.UserId);
                dashboard.AllEmails = await _emailListRepository.GetEmailsCountByUserId(request.UserId);
                dashboard.MonthlyStats = await _emailresponse.GetMonthlyStatsByUserIdAsync(request.UserId);

                //emails
                var dataresult = await _emailProjectRepository.GetDashboardStatsByUserIdAsync(request.UserId);
                // This Month
                dashboard.ThisMonthTotalProjects = dataresult.ThisMonthTotalProjects;
                dashboard.ThisMonthTotalSubmitted = dataresult.ThisMonthTotalSubmitted;
                dashboard.ThisMonthTotalDelivered = dataresult.ThisMonthTotalDelivered;
                dashboard.ThisMonthTotalOpened = dataresult.ThisMonthTotalOpened;

                // All Time
                dashboard.AllTimeTotalProjects = dataresult.AllTimeTotalProjects;
                dashboard.AllTimeTotalSubmitted = dataresult.AllTimeTotalSubmitted;
                dashboard.AllTimeTotalDelivered = dataresult.AllTimeTotalDelivered;
                dashboard.AllTimeTotalOpened = dataresult.AllTimeTotalOpened;

                // Statistics
                dashboard.TotalEmailsInSystemWithoutDuplicate = dataresult.TotalEmailsInSystemWithoutDuplicate;
                dashboard.TotalAmountSpent = dataresult.TotalAmountSpent;

                var existingWallet = await _walletRepository.GetByUserIdAsync(request.UserId);
                if (existingWallet == null)
                {
                    dashboard.Balance = existingWallet.Balance;
                }

                var existingSubscription = await _userSubscriptionRepository.GetByUserIdAsync(request.UserId);
                if (existingSubscription == null)
                {
                    dashboard.Limit = existingSubscription.RemainingEmailsForMonth;
                }

                return dashboard;

            }
        }
    }

}
