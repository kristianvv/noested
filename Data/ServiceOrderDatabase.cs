using Noested.Models;

namespace Noested.Data
{
    public class ServiceOrderDatabase
    {
        private List<ServiceOrderModel> ServiceOrders { get; set; } = new List<ServiceOrderModel>();
        private List<ChecklistDTO> ChecklistsDto { get; set; } = new List<ChecklistDTO>();
        private List<Customer> Customers { get; set; } = new List<Customer>();
        private int LastOrderNumber { get; set; } = 0;

        // For ServiceOrders: GetById
        public Task<ServiceOrderModel?> GetOrderByIdAsync(int id)
        {
            var result = ServiceOrders.Find(o => o.ServiceOrderID == id);
            return Task.FromResult(result);
        }

        // GetAll
        public Task<IEnumerable<ServiceOrderModel>> GetAllServiceOrdersAsync()
        {
            return Task.FromResult<IEnumerable<ServiceOrderModel>>(ServiceOrders);
        }
        // Add
        public Task AddServiceOrderAsync(ServiceOrderModel order)
        {
            LastOrderNumber++;
            order.OrderRecieved = DateTime.Now;
            order.ServiceOrderID = LastOrderNumber;
            ServiceOrders.Add(order);
            return Task.FromResult(0);
        }
        // Update
        public Task UpdateOrderAsync(ServiceOrderModel updatedOrder)
        {
            var existingOrder = ServiceOrders.Find(o => o.ServiceOrderID == updatedOrder.ServiceOrderID);
            if (existingOrder != null)
            {
                existingOrder.OrderCompleted = updatedOrder.OrderCompleted;
                existingOrder.SerialNumber = updatedOrder.SerialNumber;
                existingOrder.ModelYear = updatedOrder.ModelYear;
                existingOrder.Warranty = updatedOrder.Warranty;
                existingOrder.RepairDescription = updatedOrder.RepairDescription;
                existingOrder.WorkHours = updatedOrder.WorkHours;
                existingOrder.Customer = updatedOrder.Customer;
                existingOrder.Customer!.CustomerID = updatedOrder.Customer!.CustomerID;
                existingOrder.Checklists = updatedOrder.Checklists;
            }
            return Task.FromResult(0);
        }

        // For Checklists: Add
        public Task AddChecklistAsync(ChecklistDTO model)
        {
            ChecklistsDto.Add(model);
            return Task.FromResult(0);
        }

        // GetAll
        public Task<IEnumerable<ChecklistDTO>> GetAllChecklistsAsync()
        {
            return Task.FromResult<IEnumerable<ChecklistDTO>>(ChecklistsDto);
        }

        // For Customers: Add
        public Task AddCustomerAsync(Customer newCustomer)
        {
            Customers.Add(newCustomer);
            return Task.FromResult(0);
        }

        // GetAll
        public Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return Task.FromResult<IEnumerable<Customer>>(Customers);
        }
    }
}
