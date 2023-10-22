using Noested.Data;
using Noested.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        //
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

        //
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

        //
        public async Task<bool> CreateNewServiceOrderAsync(ServiceOrderModel newOrder, int? existingCustomerId)
        {
            _logger.LogInformation("CreateNewServiceOrderAsync(): Called");
            if (existingCustomerId.HasValue)
            {
                newOrder.Customer.CustomerID = existingCustomerId.Value;
            }
            else
            {
                if (newOrder.Customer != null)
                {
                    await _repository.AddCustomerAsync(newOrder.Customer);
                }
                else
                {
                    return false;
                }
            }

            await _repository.AddServiceOrderAsync(newOrder);
            return true;
        }

        //
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

        //
        public async Task<bool> UpdateCompletedOrderAsync(ServiceOrderModel completedOrder, IFormCollection? form)
        {
            _logger.LogInformation("UpdateCompletedOrderAsync(): Called");

            if (completedOrder == null)
            {
                _logger.LogError("UpdateCompletedOrderAsync(): completedOrder is null");
                return false;
            }

            if (form == null)
            {
                _logger.LogError("UpdateCompletedOrderAsync(): form is null");
                return false;
            }

            var existingOrder = await _repository.GetOrderByIdAsync(completedOrder.ServiceOrderID);
            if (existingOrder == null)
            {
                _logger.LogError("UpdateCompletedOrderAsync(): Requested Order Does Not Exist");
                return false;
            }

            await FieldUpdateService.UpdateFieldsAsync(existingOrder, completedOrder, form);

            _logger.LogInformation("UpdateCompletedOrderAsync(): Order updated successfully");
            return true;
        }
    }
}
