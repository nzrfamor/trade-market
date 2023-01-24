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
    public class ReceiptsController : ControllerBase
    {
        readonly IReceiptService receiptService;
        public ReceiptsController(IReceiptService receiptService)
        {
            this.receiptService = receiptService;
        }

        // GET/api/receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetAll()
        {
            return new ObjectResult(await receiptService.GetAllAsync());
        }

        // GET/api/receipts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetById(int id)
        {
            return new ObjectResult(await receiptService.GetByIdAsync(id));
        }

        // GET/api/receipts/{id}/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetAllDetails(int id)
        {
            return new ObjectResult(await receiptService.GetReceiptDetailsAsync(id));
        }

        // GET/api/receipts/{id}/sum
        [HttpGet("{id}/sum")]
        public async Task<ActionResult<decimal>> GetSum(int id)
        {
            return new ObjectResult(await receiptService.ToPayAsync(id));
        }

        // GET/api/receipts/period
        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return new ObjectResult(await receiptService.GetReceiptsByPeriodAsync(startDate, endDate));
        }

        // POST/api/receipts
        [HttpPost]
        public async Task<ActionResult> AddReceipt([FromBody] ReceiptModel receipt)
        {
            try
            {
                await receiptService.AddAsync(receipt);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(receipt);
        }

        // PUT/api/receipts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReceipt(int id, [FromBody] ReceiptModel receipt)
        {
            try
            {
                receipt.Id = id;
                await receiptService.UpdateAsync(receipt);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(receipt);
        }

        // PUT/api/receipts/{id}/products/add/{productId}/{quantity}
        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> AddProductToReceipt(int id, int productId, int quantity)
        {
            try
            {
                await receiptService.AddProductAsync(productId, id, quantity);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        // PUT/api/receipts/{id}/products/remove/{productId}/{quantity}
        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProductToReceipt(int id, int productId, int quantity)
        {
            try
            {
                await receiptService.RemoveProductAsync(productId, id, quantity);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        // PUT/api/receipts/{id}/checkout
        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> SetTrueToCheckOut(int id)
        {
            try
            {
                await receiptService.CheckOutAsync(id);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        // DELETE/api/receipts/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await receiptService.DeleteAsync(id);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}

