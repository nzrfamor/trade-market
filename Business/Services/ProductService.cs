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
    public class ProductService : IProductService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task AddAsync(ProductModel model)
        {
            if(model.Price < 0)
            {
                throw new MarketException("Price cannot be negative!");
            }
            if(model.ProductName == string.Empty)
            {
                throw new MarketException("ProductName cannot be empty!");
            }
            var product = _mapper.Map<Product>(model);
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel.CategoryName == string.Empty)
            {
                throw new MarketException("CategoryName cannot be empty!");
            }
            var productCategory = _mapper.Map<ProductCategory>(categoryModel);
            await _unitOfWork.ProductCategoryRepository.AddAsync(productCategory);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var products = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryModel>>(products);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();

            if(filterSearch.CategoryId != null)
            {
                products = products.Where(p => p.ProductCategoryId == filterSearch.CategoryId);
            }
            if (filterSearch.MinPrice != null)
            {
                products = products.Where(p => p.Price >= filterSearch.MinPrice);
            }
            if (filterSearch.MaxPrice != null)
            {
                products = products.Where(p => p.Price <= filterSearch.MaxPrice);
            }

            return _mapper.Map<IEnumerable<ProductModel>>(products);

        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductModel>(product);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            if (model.ProductName == string.Empty)
            {
                throw new MarketException("ProductName cannot be empty!");
            }
            var product = _mapper.Map<Product>(model);
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel.CategoryName == string.Empty)
            {
                throw new MarketException("CategoryName cannot be empty!");
            }
            var productCategory = _mapper.Map<ProductCategory>(categoryModel);
            _unitOfWork.ProductCategoryRepository.Update(productCategory);
            await _unitOfWork.SaveAsync();
        }
    }
}
