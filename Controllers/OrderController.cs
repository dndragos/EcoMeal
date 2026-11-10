using BlazorApp1.Entities;
using BlazorApp1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class OrderController(IOrderService orderService) : ControllerBase
        {
            [HttpGet]
            public async Task<ActionResult<List<Order>>> GetAll()
            {
                return await orderService.GetAll();
            }

            [HttpPost]
            public async Task<ActionResult> AddAsync(Order order)
            {
                await orderService.AddAsync(order);
                return Ok();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Order?>> GetById(Guid id)
            {
                return await orderService.GetByIdAsync(id);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateAsync(Guid id, StatusEnum status)
            {
                await orderService.UpdateStatusAsync(id, status);
                return Ok();
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteAsync(Guid id)
            {
                await orderService.DeleteAsync(id);
                return Ok();
            }
        }
}
