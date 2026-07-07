using BlazorApp1.Entities;
using BlazorApp1.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlazorApp1.Services.Interfaces;

namespace BlazorApp1.Controllers
{
    [ApiController]
    public class BusinessController(IBusinessService businessSerivce) : ControllerBase
    {
        public async Task<ActionResult<List<Business>>> GetAll()
        {
            return await businessSerivce.GetAll();
        }
        public async Task<ActionResult> AddAsync(Business business)
        {
            await businessSerivce.AddAsync(business);
            return Ok();

        }
    
}
}
