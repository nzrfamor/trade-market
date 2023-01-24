using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly TradeMarketDbContext db;
        ICustomerRepository customerRepository;
        IPersonRepository personRepository;
        IProductRepository productRepository;
        IProductCategoryRepository productCategoryRepository;
        IReceiptRepository receiptRepository;
        IReceiptDetailRepository receiptDetailRepository;

        public UnitOfWork(TradeMarketDbContext _db)
        {
            this.db = _db;
        }
        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (customerRepository == null) customerRepository = new CustomerRepository(db);
                return customerRepository;
            }
        }

        public IPersonRepository PersonRepository
        {
            get
            {
                if (personRepository == null) personRepository = new PersonRepository(db);
                return personRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null) productRepository = new ProductRepository(db);
                return productRepository;
            }
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                if (productCategoryRepository == null) productCategoryRepository = new ProductCategoryRepository(db);
                return productCategoryRepository;
            }
        }

        public IReceiptRepository ReceiptRepository
        {
            get
            {
                if (receiptRepository == null) receiptRepository = new ReceiptRepository(db);
                return receiptRepository;
            }
        }

        public IReceiptDetailRepository ReceiptDetailRepository
        {
            get
            {
                if (receiptDetailRepository == null) receiptDetailRepository = new ReceiptDetailRepository(db);
                return receiptDetailRepository;
            }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
