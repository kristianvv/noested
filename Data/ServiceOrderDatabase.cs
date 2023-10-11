using Noested.Models;
using Noested.Models.DTOs;


namespace Noested.Data
{
    public class ServiceOrderDatabase
    {
        public List<ServiceOrderModel> ServiceOrders { get; private set; } = new List<ServiceOrderModel>();
        public List<ChecklistDto> ChecklistsDto { get; private set; } = new List<ChecklistDto>();
        public int LastOrderNumber { get; set; } = 0;

        /// Summary: Adds a checklist to the database.
        /// <param name="model">The checklist model.</param>
        public void AddChecklist(ChecklistDto model)
        {
            ChecklistsDto.Add(model);
        }

        /// Summary: Retrieves all checklists from the database.
        /// Returns: An IEnumerable of ChecklistDto.</returns>
        public IEnumerable<ChecklistDto> GetAllChecklists()
        {
            return ChecklistsDto;
        }

        /// Summary: Retrieves all service orders from the database.
        /// Returns: An IEnumerable of ServiceOrderModel.</returns>
        public IEnumerable<ServiceOrderModel> GetAllServiceOrders()
        {
            return ServiceOrders;
        }

        /// Summary: Adds a service order to the database and updates its OrderNumber.
        /// <param name="order">The service order model.</param>
        public void AddServiceOrder(ServiceOrderModel order)
        {
            LastOrderNumber++;
            order.OrderNumber = LastOrderNumber;
            ServiceOrders.Add(order);
        }

        /// Summary: Retrieves a service order by its OrderNumber.
        /// <param name="id">The OrderNumber.</param>
        /// Returns: A ServiceOrderModel or null.</returns>
        public ServiceOrderModel? GetOrderById(int id)
        {
            return ServiceOrders.FirstOrDefault(o => o.OrderNumber == id);
        }

        /// Summary: Updates an existing service order.
        /// <param name="updatedOrder">The updated service order model.</param>
        public void UpdateOrder(ServiceOrderModel updatedOrder)
        {
            var existingOrder = ServiceOrders.FirstOrDefault(o => o.OrderNumber == updatedOrder.OrderNumber);
            if (existingOrder != null)
            {
                foreach (var property in existingOrder.GetType().GetProperties())
                {
                    if (property.CanWrite)
                    {
                        var newValue = property.GetValue(updatedOrder);
                        if (newValue != null)
                        {
                            property.SetValue(existingOrder, newValue);
                        }
                    }
                }
            }
        }
    }
}
