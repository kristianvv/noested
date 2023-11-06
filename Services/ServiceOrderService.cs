using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Models;
using static Noested.Models.ServiceOrder;

namespace Noested.Services
{
    public class ServiceOrderService
    {
        private readonly IServiceOrderRepository _repository;
        private readonly ChecklistService _checklistService;
        private readonly ILogger<ServiceOrderService> _logger;

        public ServiceOrderService(IServiceOrderRepository repository, ChecklistService checklistService, ILogger<ServiceOrderService> logger)
        {
            _repository = repository;
            _logger = logger;
            _checklistService = checklistService;
        }

        // GET SO
        public async Task<ServiceOrder> GetOrderByIdAsync(int id)
        {
            var order = await _repository.GetOrderByIdAsync(id);
            return order ?? throw new InvalidOperationException($"SERVICE ORDER ({id}) NOT FOUND");
        }

        // VIEW FOR OPEN SO
        public async Task<OrderViewModel> PopulateOrderViewModel(int id)
        {
            var order = await GetOrderByIdAsync(id);
            int checklistId = order.OrderId;

            var viewModel = new OrderViewModel
            {
                FillOrder = order,
                NewChecklist = await GetChecklistSubClass(order.Product, checklistId)
            };

            return viewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productType"></param>
        /// <param name="checklistId"></param>
        /// <returns></returns>
        private async Task<Checklist> GetChecklistSubClass(ProductType productType, int checklistId)
        {
            Checklist checklistDetails = new();

            switch (productType)
            {
                case ProductType.Winch:
                    checklistDetails = await _checklistService.GetWinchChecklistByIdAsync(checklistId);
                    break;
                case ProductType.LiftEquip:
                    _logger.LogError("NEED TO ADD THIS CHECKLIST TO MODEL, DbSET & PARTIAL VIEW");
                    break;
                case ProductType.WoodEquip:
                    _logger.LogError("NEED TO ADD THIS CHECKLIST TO MODEL, DbSET & PARTIAL VIEW");
                    break;
                case ProductType.TractorShears:
                    _logger.LogError("NEED TO ADD THIS CHECKLIST TO MODEL, DbSET & PARTIAL VIEW");
                    break;
                case ProductType.TimberTrailer:
                    _logger.LogError("NEED TO ADD THIS CHECKLIST TO MODEL, DbSET & PARTIAL VIEW");
                    break;
                case ProductType.SandBlaster:
                    _logger.LogError("NEED TO ADD THIS CHECKLIST TO MODEL, DbSET & PARTIAL VIEW");
                    break;
                case ProductType.SnowBloPlo:
                    _logger.LogError("NEED TO ADD THIS CHECKLIST TO MODEL, DbSET & PARTIAL VIEW");
                    break;
                // Add cases for other product types
                default:
                    _logger.LogError($"No checklist implemented for product type: {productType}");
                    break;
            }

            return checklistDetails;
        }


        /// <summary>
        ///     CREATE SERVICE ORDER
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task<bool> CreateNewServiceOrderAsync(OrderViewModel viewModel)
        {
            IEnumerable<ServiceOrder> allServiceOrders = await _repository.GetAllServiceOrdersAsync(); // Duplicate Order?
            IEnumerable<Customer> allCustomers = await _repository.GetAllCustomersAsync(); // Duplicate Customer?

            ServiceOrder? newOrder = viewModel.FillOrder;
            Customer? newCustomer = viewModel.NewCustomer;
            Checklist? newChecklist = viewModel.NewChecklist;

            if (newOrder == null || newChecklist == null)
            {
                throw new InvalidOperationException("CreateNewSOA: Order or Checklist is Null!");
            }

            var viewModelJson = JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { WriteIndented = true }); // Debug
            _logger.LogInformation("CreateOrder ViewModel: {ViewModelJson}", viewModelJson); // Debug

            var duplicateOrder = allServiceOrders.FirstOrDefault(o =>
                newOrder != null &&
                o.ProductName == newOrder.ProductName &&
                o.SerialNumber == newOrder.SerialNumber &&
                o.OrderReceived.Date == newOrder.OrderReceived.Date // Same day?
            );

            if (duplicateOrder != null)
            {
                _logger.LogError("Duplicate ServiceOrder found. Skipping this order.");
                return false;
            }

            if (newOrder!.CustomerId == 0) // New customer?
            {
                Customer? existingCustomer = allCustomers.FirstOrDefault(c => c.Email == newCustomer!.Email);
                if (existingCustomer != null)
                {
                    newOrder.CustomerId = existingCustomer.CustomerId;
                }
                else
                {
                    await _repository.AddCustomerAsync(newCustomer!);
                    newOrder.CustomerId = newCustomer!.CustomerId;
                }
            }
                switch (newOrder.Product)
                {
                    case ProductType.Winch:
                        var winchChecklist = new WinchChecklist();
                        {
                            winchChecklist.ProductType = newChecklist!.ProductType;
                            winchChecklist.PreparedBy = newChecklist.PreparedBy;
                            winchChecklist.ServiceProcedure = newChecklist.ServiceProcedure;
                        };
                        newOrder.Checklist = winchChecklist;
                        break;
                        // case ProductType.LiftEquip:
                }
            

            await _repository.AddServiceOrderAsync(newOrder); // Save order


            _logger.LogInformation("CreateOrder ViewModel: {ViewModelJson}", viewModelJson); // Debug
            return true;

        }
        /// <summary>
        /// ALL SERVICEORDERS
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IEnumerable<ServiceOrder>> GetAllServiceOrdersAsync()
        {
            var allServiceOrders = await _repository.GetAllServiceOrdersAsync();
            if (allServiceOrders == null || !allServiceOrders.Any())
            {
                throw new InvalidOperationException("Null or no ServiceOrders in database");
            }
            return allServiceOrders;
        }
        

        

        // UPDATE SERVICE ORDER
        public async Task<bool> UpdateCompletedOrderAsync(OrderViewModel completedOrder)
        {
            if (completedOrder == null)
            {
                return false;
            }

            var existingOrder = await _repository.GetOrderByIdAsync(completedOrder.FillOrder!.OrderId);
            if (existingOrder == null)
            {
                return false;
            }
            await _repository.UpdateOrderAsync(existingOrder);

            return true;
        }

       
    }
}
