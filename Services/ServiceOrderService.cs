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
        public async Task<bool> CreateNewServiceOrderAsync(ServiceOrder newOrder)
        {
            _logger.LogInformation("CreateNewServiceOrderAsync(): Called");

            if (newOrder.CustomerId == 0)
            {
                await _repository.AddCustomerAsync(newOrder.Customer!);
                newOrder.CustomerId = newOrder.Customer!.CustomerId;
            }

            await _repository.AddServiceOrderAsync(newOrder);
            _logger.LogInformation("CreateNewServiceOrderAsync(): Order created successfully");
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
