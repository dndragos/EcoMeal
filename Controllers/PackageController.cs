using BlazorApp1.Entities;
using Microsoft.AspNetCore.Mvc;
using BlazorApp1.Services.Interfaces;

namespace BlazorApp1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController(IPackageService packageService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Package>>> GetAll()
        {
            return await packageService.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(Package package)
        {
            await packageService.AddAsync(package);
            return Ok();
        }
    }
}
