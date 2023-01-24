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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        readonly TradeMarketDbContext db;
        public ProductCategoryRepository(TradeMarketDbContext _db)
        {
            this.db = _db;
        }

        public async Task AddAsync(ProductCategory entity)
        {
            await db.ProductCategories.AddAsync(entity);
        }

        public void Delete(ProductCategory entity)
        {
            db.ProductCategories.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            db.ProductCategories.Remove(await db.ProductCategories.FirstOrDefaultAsync(pr => pr.Id == id));
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await db.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await db.ProductCategories.FirstOrDefaultAsync(pr => pr.Id == id);
        }

        public void Update(ProductCategory entity)
        {
            db.ProductCategories.Update(entity);
        }
    }
}
