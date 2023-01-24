using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task AddAsync(CustomerModel model)
        {
            if (model == null)
            {
                throw new MarketException("CustomerModel object cannot be null!");
            }
            if (model.Name == null || model.Name.Length == 0)
            {
                throw new MarketException("Name cannot be empty!");
            }
            if (model.Surname == null || model.Surname.Length == 0)
            {
                throw new MarketException("Surname cannot be empty!");
            }
            if (model.BirthDate >= DateTime.Now || model.BirthDate < DateTime.Parse("1911-1-1"))
            {
                throw new MarketException("Incorrect BirthDate value!");
            }
            if (model.DiscountValue < 0 || model.DiscountValue > 100)
            {
                throw new MarketException("Incorrect Discount value!");
            }
            var customer = _mapper.Map<Customer>(model);
            await _unitOfWork.CustomerRepository.AddAsync(customer);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customer = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CustomerModel>>(customer);  
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<CustomerModel>(customer);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CustomerModel>>(customers.Where(c => c.Receipts.SelectMany(r => r.ReceiptDetails
                                                                                          .Select(rd => rd.ProductId))
                                                                                          .Contains(productId)));
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            if (model == null)
            {
                throw new MarketException("CustomerModel object cannot be null!");
            }
            if (model.Name == null || model.Name.Length == 0)
            {
                throw new MarketException("Name cannot be empty!");
            }
            if (model.Surname == null || model.Surname.Length == 0)
            {
                throw new MarketException("Surname cannot be empty!");
            }
            if (model.BirthDate >= DateTime.Now || model.BirthDate < DateTime.Parse("1911-1-1"))
            {
                throw new MarketException("Incorrect BirthDate value!");
            }
            if (model.DiscountValue < 0 || model.DiscountValue > 100)
            {
                throw new MarketException("Incorrect Discount value!");
            }
            var customer = _mapper.Map<Customer>(model);
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.SaveAsync();
        }
    }
}
