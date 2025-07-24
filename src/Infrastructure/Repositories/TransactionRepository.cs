using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
     
    public sealed class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly AppDBContext _context;

        public TransactionRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
