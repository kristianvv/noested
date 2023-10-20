using Noested.Models;

namespace Noested.Services
{
	public class FieldUpdateService
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="existingOrder"></param>
        /// <param name="completedOrder"></param>
        public static async Task UpdateFieldsAsync(ServiceOrderModel existingOrder, ServiceOrderModel completedOrder)
        {
            if (existingOrder == null || completedOrder == null)
            {
                throw new ArgumentException("Existing order or completed order cannot be null");
            }

            existingOrder.ServiceOrderID = completedOrder.ServiceOrderID;
            existingOrder.ServiceOrderStatus = completedOrder.ServiceOrderStatus;
            existingOrder.OrderRecieved = completedOrder.OrderRecieved;
            existingOrder.OrderCompleted = completedOrder.OrderCompleted;
            existingOrder.SerialNumber = completedOrder.SerialNumber;
            existingOrder.ModelYear = completedOrder.ModelYear;
            existingOrder.Warranty = completedOrder.Warranty;
            existingOrder.RepairDescription = completedOrder.RepairDescription;
            existingOrder.WorkHours = completedOrder.WorkHours;
            existingOrder.Customer = completedOrder.Customer;
            existingOrder.Customer!.CustomerID = completedOrder.Customer!.CustomerID;
            existingOrder.Checklists = completedOrder.Checklists;

            await Task.CompletedTask;
        }
    }
}