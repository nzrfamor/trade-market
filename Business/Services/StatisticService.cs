using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var products = receipts.Where(r => r.CustomerId == customerId)
                                   .SelectMany(r => r.ReceiptDetails)
                                   .GroupBy(rd => rd.ProductId)
                                   .OrderByDescending(rd => rd.Sum(x => x.Quantity))
                                   .Take(productCount)
                                   .Select(rd => rd.Select(x => x.Product).First());

            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                              .SelectMany(r => r.ReceiptDetails.Where(rd => rd.Product.Category.Id == categoryId))
                              .Select(x => x.DiscountUnitPrice * x.Quantity)
                              .Sum();
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetail = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();
            var products = receiptDetail.GroupBy(rd => rd.ProductId)
                                        .OrderByDescending(rd => rd.Select(r => r.Quantity).Sum())
                                        .Select(rd => rd.Select(r => r.Product).First()).Take(productCount);
            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var customers = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                                    .GroupBy(r => r.CustomerId)
                                    .Select(r =>  new CustomerActivityModel
                                    {
                                        CustomerId = r.Key,
                                        CustomerName = r.Select(x => x.Customer.Person.Name + " " + x.Customer.Person.Surname)
                                                        .First(),
                                        ReceiptSum = r.Select(x => x.ReceiptDetails.Select(rd => rd.DiscountUnitPrice * rd.Quantity)
                                                                                   .Sum())
                                                      .Sum()
                                    })
                                    .OrderByDescending(cam => cam.ReceiptSum)
                                    .Take(customerCount);
            return _mapper.Map<IEnumerable<CustomerActivityModel>>(customers);
        }
    }
}
