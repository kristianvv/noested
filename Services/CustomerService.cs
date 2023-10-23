using Noested.Data;
using Noested.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Noested.Services
{
    public class CustomerService
    {
        private readonly IServiceOrderRepository _repository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IServiceOrderRepository repository, ILogger<CustomerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Hente alle kunder
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

        // Legge til nye kunder
        public async Task AddNewCustomerAsync(Customer newCustomer)
        {
            _logger.LogInformation("AddNewCustomerAsync(): Called");
            if (newCustomer == null)
            {
                throw new ArgumentNullException("newCustomer cannot be null");
            }
            await _repository.AddCustomerAsync(newCustomer);
        }
    }
}
