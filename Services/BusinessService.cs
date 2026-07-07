using BlazorApp1.Data;
using BlazorApp1.Entities;
using BlazorApp1.Repositories;
using BlazorApp1.Repositories.Interfaces;
using BlazorApp1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class BusinessService(IBusinessRepository businessRepository) : IBusinessService
    {
        public async Task<List<Business>> GetAll()
        {
            return await businessRepository.GetAllAsync();
        }

        public async Task AddAsync(Business business)
        {
            await businessRepository.AddAsync(business);
            await businessRepository.SaveChangesAsync();
        }
    }
}
