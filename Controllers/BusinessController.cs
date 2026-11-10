using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlazorApp1.Services.Interfaces;

namespace BlazorApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController(IBusinessService businessSerivce) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Business>>> GetAll()
        {
            return await businessSerivce.GetAll();
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<List<Business>>> GetPaginated([FromQuery] string? searchTerm, [FromQuery] BusinessTypeEnum type, [FromQuery] int skip, [FromQuery] int take)
        {
            return await businessSerivce.GetPaginatedAsync(searchTerm, type, skip, take);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(Business business)
        {
            await businessSerivce.AddAsync(business);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Business?>> GetById(Guid id)
        {
            return await businessSerivce.GetByIdAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, Business business)
        {
            await businessSerivce.UpdateAsync(id, business);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            await businessSerivce.DeleteAsync(id);
            return Ok();
        }
    }
}
