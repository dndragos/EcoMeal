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

        public async Task<Business?> GetByIdAsync(Guid id)
        {
            return await businessRepository.GetById(id);
        }

        public async Task UpdateAsync(Guid id, Business updatedBusiness)
        {
            var business = await businessRepository.GetById(id);
            if (business != null)
            {
                business.Name = updatedBusiness.Name;
                business.Description = updatedBusiness.Description;
                business.Address = updatedBusiness.Address;
                business.ImageUrl = updatedBusiness.ImageUrl;
                business.BusinessTypeId = updatedBusiness.BusinessTypeId;
                
                await businessRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await businessRepository.DeleteAsync(id);
            await businessRepository.SaveChangesAsync();
        }
    }
}
