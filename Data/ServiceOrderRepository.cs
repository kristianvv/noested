using Noested.Models;

namespace Noested.Data
{
    public interface IServiceOrderRepository
    {
        Task<IEnumerable<ServiceOrderModel>> GetAllServiceOrdersAsync();
        Task<ServiceOrderModel?> GetOrderByIdAsync(int id);
        Task AddServiceOrderAsync(ServiceOrderModel newOrder);
        Task UpdateOrderAsync(ServiceOrderModel updatedOrder);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer newCustomer);
    }

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly ServiceOrderDatabase _database;

        public ServiceOrderRepository(ServiceOrderDatabase database)
        {
            _database = database;
        }
        // For ServiceOrders: GetAll
        public async Task<IEnumerable<ServiceOrderModel>> GetAllServiceOrdersAsync()
        {
            return await _database.GetAllServiceOrdersAsync();
        }
        // GetById
        public async Task<ServiceOrderModel?> GetOrderByIdAsync(int id)
        {
            return await _database.GetOrderByIdAsync(id);
        }
        // Add
        public async Task AddServiceOrderAsync(ServiceOrderModel newOrder)
        {
            await _database.AddServiceOrderAsync(newOrder);
        }
        // Update
        public async Task UpdateOrderAsync(ServiceOrderModel updatedOrder)
        {
            await _database.UpdateOrderAsync(updatedOrder);
        }
        // For Customers: GetAll
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _database.GetAllCustomersAsync();
        }
        // Add
        public async Task AddCustomerAsync(Customer newCustomer)
        {
            await _database.AddCustomerAsync(newCustomer);
        }




    }
}
