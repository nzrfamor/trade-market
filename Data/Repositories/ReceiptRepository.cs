using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        readonly TradeMarketDbContext db;
        public ReceiptRepository(TradeMarketDbContext _db)
        {
            this.db = _db;
        }

        public async Task AddAsync(Receipt entity)
        {
            await db.Receipts.AddAsync(entity);
        }

        public void Delete(Receipt entity)
        {
            db.Receipts.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            db.Receipts.Remove(await db.Receipts.FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await db.Receipts.ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await db.Receipts.Include(r => r.Customer)
                                    .ThenInclude(c => c.Person)
                                    .Include(r => r.ReceiptDetails)
                                    .ThenInclude(rd => rd.Product)
                                    .ThenInclude(p => p.Category)
                                    .ToListAsync();
        }

        public async Task<Receipt> GetByIdAsync(int id)
        {
            return await db.Receipts.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await db.Receipts.Include(r => r.Customer)
                                    .ThenInclude(c => c.Person)
                                    .Include(r => r.ReceiptDetails)
                                    .ThenInclude(rd => rd.Product)
                                    .ThenInclude(p => p.Category)
                                    .FirstOrDefaultAsync(r => r.Id == id);
        }

        public void Update(Receipt entity)
        {
            db.Receipts.Update(entity);
        }
    }
}
