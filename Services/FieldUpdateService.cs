using Noested.Models;

namespace Noested.Services
{
    public static class FieldUpdateService
    {
        /// Summary: Helper method to update the fields of an existing order based on a completed order.
        /// <param name="existingOrder">The existing order to update.</param>
        /// <param name="completedOrder">The completed order with new data.</param>
        public static void UpdateFields(ServiceOrderModel existingOrder, ServiceOrderModel completedOrder)
        {
            if (existingOrder == null || completedOrder == null)
            {
                throw new ArgumentException("Existing order or completed order cannot be null");
            }

            existingOrder.ProductName = completedOrder.ProductName;
            existingOrder.ProductType = completedOrder.ProductType;
            existingOrder.WeekNumber = completedOrder.WeekNumber;
            existingOrder.DayOfWeek = completedOrder.DayOfWeek;
            existingOrder.OrderStatus = completedOrder.OrderStatus;
            existingOrder.ContactPerson = completedOrder.ContactPerson;
            existingOrder.Address = completedOrder.Address;
            existingOrder.PhoneNumber = completedOrder.PhoneNumber;
            existingOrder.Email = completedOrder.Email;
            existingOrder.CustomerComment = completedOrder.CustomerComment;
            existingOrder.AgreedDeliveryDate = completedOrder.AgreedDeliveryDate;
            existingOrder.AgreedFinishedDate = completedOrder.AgreedFinishedDate;
            existingOrder.ReceivedProductDate = completedOrder.ReceivedProductDate;
            existingOrder.CompletedServiceDate = completedOrder.CompletedServiceDate;
            existingOrder.HoursToComplete = completedOrder.HoursToComplete;
            existingOrder.OpenedAt = completedOrder.OpenedAt;
            existingOrder.TimeToOpen = completedOrder.TimeToOpen;
            // existingOrder.CompletedAt = completedOrder.CompletedAt;
            existingOrder.TimeToComplete = completedOrder.TimeToComplete;
            existingOrder.Checklists = completedOrder.Checklists;
        }
    }

}