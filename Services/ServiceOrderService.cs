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

        // Index for ServiceOrdersController
        public async Task<IEnumerable<ServiceOrder>> FetchAllServiceOrdersAsync()
        {
            _logger.LogInformation("FetchAllServiceOrdersAsync(): Called");
            var allServiceOrders = await _repository.GetAllServiceOrdersAsync();
            if (allServiceOrders == null || !allServiceOrders.Any())
            {
                _logger.LogError("FetchAllServiceOrdersAsync(): No ServiceOrders found");
                throw new InvalidOperationException("Null or no ServiceOrders in database");
            }
            return allServiceOrders;
        }
        
        // CreateOrder (POST) for ServiceOrdersController
        public async Task<bool> CreateNewServiceOrderAsync(CreateOrderViewModel viewModel)
        {
            ServiceOrder? newOrder = viewModel.NewServiceOrder;
            Customer? newCustomer = viewModel.NewCustomer;

            IEnumerable<ServiceOrder> allServiceOrders = await _repository.GetAllServiceOrdersAsync(); // Duplicate Order?
            var duplicateOrder = allServiceOrders.FirstOrDefault(o =>
                o.ProductName == newOrder!.ProductName &&
                o.SerialNumber == newOrder!.SerialNumber &&
                o.OrderReceived.Date == newOrder!.OrderReceived.Date // Same day?
            );
                
            if (duplicateOrder != null)
            {
                _logger.LogError("Duplicate ServiceOrder found. Skipping this order.");
                return false;
            }
            
            if (newOrder!.CustomerId == 0 && newCustomer == null) // New Customer selected but fields are null
            {
                _logger.LogError("Customer information is incomplete or null.");
                return false;
            }
            
            if (newOrder.CustomerId != 0) // Existing Customer Selected
            {
                _logger.LogInformation("Existing Customer Selected");
                await _repository.AddServiceOrderAsync(newOrder);
            }
            else // Selected to Register New Customer
            {
                _logger.LogInformation("Attempting to Register New Customer");
                IEnumerable<Customer> allCustomers = await _repository.GetAllCustomersAsync();
                Customer? existingCustomer = allCustomers.FirstOrDefault(c => c.Email == newCustomer!.Email); 
                if (existingCustomer != null) 
                {
                    _logger.LogInformation("Found Existing Customer in Database with the same Email as the New Customer");
                    newCustomer!.CustomerId = existingCustomer.CustomerId;
                }
                else
                {
                    _logger.LogInformation("Attempting to add new customer");
                    await _repository.AddCustomerAsync(newCustomer!);
                    newOrder.CustomerId = newCustomer!.CustomerId;
                }
                await _repository.AddServiceOrderAsync(newOrder);
            }

            _logger.LogInformation("CreateNewServiceOrderAsync(): Order created successfully");
            return true;
        }





        // 
        public async Task<bool> UpdateCompletedOrderAsync(ServiceOrder completedOrder, IFormCollection? form)
        {
            _logger.LogInformation("UpdateCompletedOrderAsync(): Called");

            if (completedOrder == null || form == null)
            {
                _logger.LogError("UpdateCompletedOrderAsync(): Null arguments");
                return false;
            }

            var existingOrder = await _repository.GetOrderByIdAsync(completedOrder.OrderId);
            if (existingOrder == null)
            {
                _logger.LogError("UpdateCompletedOrderAsync(): Order not found");
                return false;
            }

            // await FieldUpdateService.UpdateFieldsAsync(existingOrder, completedOrder, form);

            await _repository.UpdateOrderAsync(existingOrder);

            _logger.LogInformation("UpdateCompletedOrderAsync(): Order updated successfully");
            return true;
        }


        //
        public async Task<ServiceOrder> FetchServiceOrderByIdAsync(int id)
        {
            _logger.LogInformation($"FetchServiceOrderByIdAsync({id}): Called");
            var order = await _repository.GetOrderByIdAsync(id);
            if (order == null)
            {
                _logger.LogError($"FetchServiceOrderByIdAsync({id}): Order not found");
                throw new InvalidOperationException("Order not found");
            }
            return order;
        }
    }
}
