using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.WalletCommands
{
    public sealed class AddWalletCommand : IRequest
    {
        public AddWalletCommand(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }

    }

    public class AddWalletCommandHandler : IRequestHandler<AddWalletCommand>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly IPlanRepository _plan;

        public AddWalletCommandHandler(IWalletRepository walletRepository, IUserSubscriptionRepository userSubscriptionRepository, IPlanRepository plan)
        {
            _walletRepository = walletRepository;
            _userSubscriptionRepository = userSubscriptionRepository;
            _plan = plan;
        }

        public async Task Handle(AddWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // ✅ Check if wallet exists before adding a new one
                var existingWallet = await _walletRepository.GetByUserIdAsync(request.UserId);
                if (existingWallet == null)
                {
                    Wallet wallet = new Wallet
                    {
                        Balance = 0,
                        UserId = request.UserId,
                    };
                    await _walletRepository.AddAsync(wallet);
                }
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Error while adding wallet: {ex.Message}");
            }

            try
            {
                // ✅ Check if the user already has an active subscription
                var existingSubscription = await _userSubscriptionRepository.GetByUserIdAsync(request.UserId);
                if (existingSubscription == null)
                {
                    var allPlans = await _plan.GetAllAsync();
                    var zeroPlan = allPlans.FirstOrDefault(x => x.Price == 0);

                    if (zeroPlan != null)
                    {
                        UserSubscription sub = new UserSubscription
                        {
                            UserId = request.UserId,
                            SubscriptionStartDate = DateTime.UtcNow,
                            SubscriptionEndDate = DateTime.UtcNow.AddDays(30),
                            PlanId = zeroPlan.Id
                        };
                        await _userSubscriptionRepository.AddAsync(sub);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Error while adding subscription: {ex.Message}");
            }
        }
    }
}
