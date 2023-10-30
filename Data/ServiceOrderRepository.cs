using System;
using Microsoft.EntityFrameworkCore;
using Noested.Models;

namespace Noested.Data
{
    public interface IServiceOrderRepository
    {
        Task<IEnumerable<ServiceOrder>> GetAllServiceOrdersAsync();
        Task<ServiceOrder?> GetOrderByIdAsync(int id);
        Task AddServiceOrderAsync(ServiceOrder newOrder);
        Task UpdateOrderAsync(ServiceOrder updatedOrder);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer newCustomer);
    }

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly ApplicationDbContext _database;

        public ServiceOrderRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<ServiceOrder>> GetAllServiceOrdersAsync()
        {
            return await _database.ServiceOrder.ToListAsync();
        }

        public async Task<ServiceOrder?> GetOrderByIdAsync(int id)
        {
            return await _database.ServiceOrder.FindAsync(id);
        }

        public async Task AddServiceOrderAsync(ServiceOrder newOrder)
        {
            await _database.ServiceOrder.AddAsync(newOrder);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(ServiceOrder updatedOrder)
        {
            _database.ServiceOrder.Update(updatedOrder);
            await _database.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _database.Customer.ToListAsync();
        }

        public async Task AddCustomerAsync(Customer newCustomer)
        {
            await _database.Customer.AddAsync(newCustomer);
            await _database.SaveChangesAsync();
        }
    }
}
