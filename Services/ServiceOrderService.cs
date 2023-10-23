using Noested.Data;
using Noested.Models;

namespace Noested.Services
{
    public class ServiceOrderService
    {
        private readonly IServiceOrderRepository _repository;
        private readonly ILogger<ServiceOrderService> _logger;

        public ServiceOrderService(IServiceOrderRepository repository, ILogger<ServiceOrderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Oppdaterer utfylt sjekkliste (Model, Form)
        public async Task<bool> UpdateCompletedOrderAsync(ServiceOrderModel completedOrder, IFormCollection? form)
        {
            _logger.LogInformation("UpdateCompletedOrderAsync(): Called");

            if (completedOrder == null)
            {
                return false;
            }

            if (form == null)
            {
                return false;
            }

            var existingOrder = await _repository.GetOrderByIdAsync(completedOrder.ServiceOrderID);
            if (existingOrder == null)
            {
                return false;
            }

            await FieldUpdateService.UpdateFieldsAsync(existingOrder, completedOrder, form);

            _logger.LogInformation("UpdateCompletedOrderAsync(): Order updated successfully");
            return true;
        }

        // Henter alle serviceordre
        public async Task<IEnumerable<ServiceOrderModel>> FetchAllServiceOrdersAsync()
        {
            _logger.LogInformation("FetchAllServiceOrdersAsync(): Called");
            var allServiceOrders = await _repository.GetAllServiceOrdersAsync();
            if (allServiceOrders == null || !allServiceOrders.Any())
            {
                throw new InvalidOperationException("Null or no ServiceOrders in database");
            }
            return allServiceOrders;
        }

        // Oppretter ny serviceordre
        public async Task<bool> CreateNewServiceOrderAsync(ServiceOrderModel newOrder)
        {
            _logger.LogInformation("CreateNewServiceOrderAsync(): Called");

            if (newOrder.CustomerID == 0) // If "New Customer" was selected from dropdown...
            {
                await _repository.AddCustomerAsync(newOrder.Customer); // Create a new Customer && update Customer.CustomerID.

                // Update the S.O.model CustomerID to the updated Customer.CustomerID value (Maintain integrity for DB)
                newOrder.CustomerID = newOrder.Customer.CustomerID;
            }

            await _repository.AddServiceOrderAsync(newOrder);
            return true;
        }

        // Henter en serviceordre med ID (int)
        public async Task<ServiceOrderModel> FetchServiceOrderByIdAsync(int id)
        {
            _logger.LogInformation("FetchServiceOrderByIdAsync(id): Called");
            var order = await _repository.GetOrderByIdAsync(id);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }
            return order;
        }



        /*
        // Henter alle kunder
        public async Task<IEnumerable<Customer>> FetchAllCustomersAsync()
        {
            _logger.LogInformation("FetchAllCustomersAsync(): Called");
            var existingCustomers = await _repository.GetAllCustomersAsync();
            if (existingCustomers == null || !existingCustomers.Any())
            {
                throw new InvalidOperationException("No existing customers found in the database.");
            }
            return existingCustomers;
        }
        */
    }
}
