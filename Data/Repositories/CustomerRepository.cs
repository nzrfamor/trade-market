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
    public class CustomerRepository : ICustomerRepository
    {
        readonly TradeMarketDbContext db;
        public CustomerRepository(TradeMarketDbContext _db)
        {
            db = _db;
        }

        public async Task AddAsync(Customer entity)
        {
            await db.Customers.AddAsync(entity);
        }

        public void Delete(Customer entity)
        {
            db.Customers.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            db.Customers.Remove(await db.Customers.FirstOrDefaultAsync(c => c.Id == id));
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await db.Customers.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await db.Customers.Include(c => c.Person)
                                     .Include(c => c.Receipts)
                                     .ThenInclude(r => r.ReceiptDetails)
                                     .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await db.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await db.Customers.Include(c => c.Person)
                                     .Include(c => c.Receipts)
                                     .ThenInclude(r => r.ReceiptDetails)
                                     .FirstOrDefaultAsync(c => c.Id == id);

        }

        public void Update(Customer entity)
        {
            db.Customers.Update(entity);
        }
    }
}
