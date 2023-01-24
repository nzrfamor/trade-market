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
    public class ProductRepository : IProductRepository
    {
        readonly TradeMarketDbContext db;
        public ProductRepository(TradeMarketDbContext _db)
        {
            this.db = _db;
        }

        public async Task AddAsync(Product entity)
        {
            await db.Products.AddAsync(entity);
        }

        public void Delete(Product entity)
        {
            db.Products.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            db.Products.Remove(await db.Products.FirstOrDefaultAsync(p => p.Id == id));
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await db.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await db.Products.Include(p => p.Category)
                                    .Include(p => p.ReceiptDetails)
                                    .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await db.Products.Include(p => p.Category)
                                    .Include(p => p.ReceiptDetails)
                                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Product entity)
        {
            db.Products.Update(entity);
        }
    }
}
