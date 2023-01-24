using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task AddAsync(ReceiptModel model)
        {
            var receipt = _mapper.Map<Receipt>(model);
            await _unitOfWork.ReceiptRepository.AddAsync(receipt);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            if (receipt == null)
            {
                throw new MarketException("Such a Receipt does not exist!");
            }

            var receiptDetail = receipt.ReceiptDetails?.FirstOrDefault(rd => rd.ProductId == productId);
            if (receiptDetail != null)
            {
                receiptDetail.Quantity += quantity;
            }
            else
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new MarketException("Such a Product does not exist!");
                }
                receiptDetail = new ReceiptDetail
                {
                    ProductId = productId,
                    ReceiptId = receiptId,
                    DiscountUnitPrice = (100 - receipt.Customer.DiscountValue) * product.Price / 100,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    Receipt = receipt,
                    Product = product
                };
                await _unitOfWork.ReceiptDetailRepository.AddAsync(receiptDetail);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            receipt.IsCheckedOut = true;
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
            await _unitOfWork.ReceiptRepository.DeleteByIdAsync(receipt.Id);
            foreach(var receiptDetail in receipt.ReceiptDetails)
            {
                _unitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var receiptModels = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ReceiptModel>>(receiptModels);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ReceiptModel>(receipt);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return _mapper.Map<IEnumerable<ReceiptDetailModel>>(receipts.ReceiptDetails);

        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ReceiptModel>>(receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate));
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

            var receiptDetail = receipt.ReceiptDetails == null ? null : receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);
            if (receiptDetail.Quantity > quantity)
            {
                receiptDetail.Quantity -= quantity;
            }
            else
                _unitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
            await _unitOfWork.SaveAsync();
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return receipt.ReceiptDetails.Select(rd => rd.DiscountUnitPrice*rd.Quantity).Sum();
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            var receipt = _mapper.Map<Receipt>(model);
            _unitOfWork.ReceiptRepository.Update(receipt);
            await _unitOfWork.SaveAsync();
        }
    }
}
