using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly IProductService productService;
        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        // GET/api/products?categoryId=1&minPrice=20&maxPrice=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetWithFilter([FromQuery] FilterSearchModel filter)
        {
            return new ObjectResult(await productService.GetByFilterAsync(filter));
        }

        // GET/api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return new ObjectResult(product);
        }

        // GET/api/products/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetAllCategories()
        {
            return new ObjectResult(await productService.GetAllProductCategoriesAsync());
        }

        // POST/api/products
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ProductModel product)
        {
            try
            {
                await productService.AddAsync(product);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(product);
        }

        // POST/api/products/categories
        [HttpPost("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> AddProductCategory([FromBody] ProductCategoryModel productCategory)
        {
            try
            {
                await productService.AddCategoryAsync(productCategory);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(productCategory);
        }

        // PUT/api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductModel product)
        {
            try
            {
                product.Id = id;
                await productService.UpdateAsync(product);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(product);
        }

        // PUT/api/products/categories/{id}
        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdateProductCategory(int id, [FromBody] ProductCategoryModel productCategory)
        {
            try
            {
                productCategory.Id = id;
                await productService.UpdateCategoryAsync(productCategory);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(productCategory);
        }

        // DELET/api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await productService.DeleteAsync(id);
            return Ok();
        }

        // DELET/api/products/{id}
        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteProductCategory(int id)
        {
            await productService.RemoveCategoryAsync(id);
            return Ok();
        }
    }
}
