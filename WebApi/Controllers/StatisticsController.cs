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
    public class StatisticsController : ControllerBase
    {
        readonly IStatisticService statisticService;
        public StatisticsController(IStatisticService statisticService)
        {
            this.statisticService = statisticService;
        }

        // GET/api/statistic/popularProducts?productCount=2
        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProducts([FromQuery] int productCount)
        {
            try
            {
                return new ObjectResult(await statisticService.GetMostPopularProductsAsync(productCount));
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET/api/statistic/customer/{id}/{productCount}
        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts(int id, int productCount)
        {
            try
            {
                return new ObjectResult(await statisticService.GetCustomersMostPopularProductsAsync(productCount, id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET/api/statistic/activity/{customerCount}?startDate=2020-7-21&endDate=2020-7-22
        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<IEnumerable<CustomerActivityModel>>> GetMostValuableCustomers(int customerCount, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                return new ObjectResult(await statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate));
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET/api/statistic/income/{categoryId}?startDate=2020-7-21&endDate=2020-7-22
        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<decimal>> GetIncomeOfCategoryInPeriod(int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                return new ObjectResult(await statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
