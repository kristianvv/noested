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
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task AddCustomerAsync(Customer newCustomer);
        Task UpdateCustomerAsync(Customer updatedCustomer);

        Task<IEnumerable<Checklist>> GetAllCheckListsAsync();
        Task<Checklist?> GetChecklistByIdAsync(int id);
        Task AddChecklistAsync(Checklist newChecklist); // Spesielle tilfeller hvor maskinen leveres og sjekkes mens ordren opprettes?
        Task UpdateChecklistAsync(Checklist updatedChecklist);
    }

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly ApplicationDbContext _database;

        public ServiceOrderRepository(ApplicationDbContext database)
        {
            _database = database;
        }
        // ServiceOrders
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
        // Customers
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _database.Customer.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _database.Customer.FindAsync(id);
        }

        public async Task AddCustomerAsync(Customer newCustomer)
        {
            await _database.Customer.AddAsync(newCustomer);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer updatedCustomer)
        {
            _database.Customer.Update(updatedCustomer);
            await _database.SaveChangesAsync();
        }
        // Checklists
        public async Task<IEnumerable<Checklist>> GetAllCheckListsAsync()
        {
            return await _database.Checklist.ToListAsync();
        }

        public async Task<Checklist?> GetChecklistByIdAsync(int id)
        {
            return await _database.Checklist.FindAsync(id);
        }

        public async Task AddChecklistAsync(Checklist newChecklist)
        {
            await _database.Checklist.AddAsync(newChecklist);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateChecklistAsync(Checklist updatedChecklist)
        {
            _database.Checklist.Update(updatedChecklist);
            await _database.SaveChangesAsync();
        }
    }
}