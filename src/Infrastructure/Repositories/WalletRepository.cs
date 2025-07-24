using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
   
    public sealed class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        private readonly AppDBContext _context;

        public WalletRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Wallet> GetByUserIdAsync(string userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
