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
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        readonly TradeMarketDbContext db;
        public ReceiptDetailRepository(TradeMarketDbContext _db)
        {
            this.db = _db;
        }

        public async Task AddAsync(ReceiptDetail entity)
        {
            await db.ReceiptsDetails.AddAsync(entity);
        }

        public void Delete(ReceiptDetail entity)
        {
            db.ReceiptsDetails.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            db.ReceiptsDetails.Remove(await db.ReceiptsDetails.FirstOrDefaultAsync(rd => rd.Id == id));
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await db.ReceiptsDetails.ToListAsync();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await db.ReceiptsDetails.Include(rd => rd.Product)
                                           .ThenInclude(pr => pr.Category)
                                           .Include(rd => rd.Receipt)
                                           .ThenInclude(r => r.Customer)
                                           .ToListAsync();
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await db.ReceiptsDetails.FirstOrDefaultAsync(rd => rd.Id == id);
        }

        public void Update(ReceiptDetail entity)
        {
            db.ReceiptsDetails.Update(entity);
        }
    }
}
