using System.Text.Json;
using Noested.Data;
using Noested.Models;

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
        public async Task<FillOrderModel> PopulateOrderViewModel(int id)
        {
            var order = await GetOrderByIdAsync(id);

            var viewModel = new FillOrderModel
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                ProductName = order.ProductName,
                ProductT = order.Product,
                OrderStatus = order.Status,
                OrderReceived = order.OrderReceived,
                OrderDescription = order.OrderDescription,
                NewChecklist = await GetChecklistSubClass(order.Product, id)
            };

            _logger.LogInformation("ORDERID {OrderId} found.", viewModel.OrderId);
            _logger.LogInformation("PRODUCTT {ProductT} found.", viewModel.ProductT);
            _logger.LogInformation("ORDERSTATUS {OrderStatus} found.", viewModel.OrderStatus);
            _logger.LogInformation("Checklist {@NewChecklist} found.", viewModel.NewChecklist);
            _logger.LogInformation("CHECK {MechDrumBearing}", viewModel.NewChecklist);


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


        
        public async Task<bool> CreateNewServiceOrderAsync(OrderViewModel viewModel)
        {
            IEnumerable<ServiceOrder> allServiceOrders = await _repository.GetAllServiceOrdersAsync(); // Duplicate Order?
            IEnumerable<Customer> allCustomers = await _repository.GetAllCustomersAsync(); // Duplicate Customer?

            ServiceOrder? newOrder = viewModel.NewOrder;
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
        ///     ALL SERVICEORDERS
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

        /// <summary>
        ///     Updates Checklist in an existing order
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCompletedOrderAsync(FillOrderModel form)
        {
            var viewModelJson = JsonSerializer.Serialize(form, new JsonSerializerOptions { WriteIndented = true }); // Debug
            _logger.LogInformation("UPDATE COMPLETED ORDER: {ViewModelJson}", viewModelJson); // Debug

            ServiceOrder? DbOrder = await _repository.GetOrderByIdAsync(form.OrderId);
            Checklist? DbChecklist = await _repository.GetChecklistByIdAsync(form.OrderId);

            if (DbOrder == null || DbChecklist == null)
            {
                _logger.LogError("Neither Order or Checklist with ID {OrderId} found.", form.OrderId);
                return false;
            }

            if (DbChecklist is WinchChecklist DbWinch)
            {
                if (form.NewChecklist is not WinchChecklist updCheck)
                {
                    _logger.LogError("Checklist ({NewChecklist}) is not of type Winch", form.NewChecklist);
                    return false;
                }

                DbWinch.MechSignature = updCheck!.MechSignature;
                DbWinch.RepairComment = updCheck.RepairComment;
                DbWinch.DateCompleted = updCheck.DateCompleted;
                DbWinch.MechBrakes = updCheck.MechBrakes;
                DbWinch.MechDrumBearing = updCheck.MechDrumBearing;
                DbWinch.MechStoragePTO = updCheck.MechStoragePTO;
                DbWinch.MechWire = updCheck.MechWire;
                DbWinch.MechChainTensioner = updCheck.MechChainTensioner;
                DbWinch.MechPinionBearing = updCheck.MechPinionBearing;
                DbWinch.MechClutch = updCheck.MechClutch;
                DbWinch.MechSprocketWedges = updCheck.MechSprocketWedges;
                DbWinch.HydCylinder = updCheck.HydCylinder;
                DbWinch.HydHydraulicBlock = updCheck.HydHydraulicBlock;
                DbWinch.HydTankOil = updCheck.HydTankOil;
                DbWinch.HydGearboxOil = updCheck.HydGearboxOil;
                DbWinch.HydBrakeCylinder = updCheck.HydBrakeCylinder;
                DbWinch.ElCableNetwork = updCheck.ElCableNetwork;
                DbWinch.ElRadio = updCheck.ElRadio;
                DbWinch.ElButtonBox = updCheck.ElButtonBox;
                DbWinch.TensionCheckBar = updCheck.TensionCheckBar;
                DbWinch.TestWinch = updCheck.TestWinch;
                DbWinch.TestTraction = updCheck.TestTraction;
                DbWinch.TestBrakes = updCheck.TestBrakes;

                await _repository.UpdateChecklistAsync(DbWinch);
            }

            DbOrder.WorkHours = form.WorkHours;
            DbOrder.Status = form.OrderStatus;
            DbOrder.OrderCompleted = form.OrderCompleted;


            await _repository.UpdateOrderAsync(DbOrder);

            return true;
        }
    }
}
