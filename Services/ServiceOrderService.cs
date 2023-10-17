using Noested.Controllers;
using Noested.Data;
using Noested.Models;
using Noested.Models.DTOs;
using Noested.Services;

namespace Noested.Services
{
    public class ServiceOrderService
    {
        private readonly IServiceOrderRepository _repository;
        private readonly ILogger<ServiceOrderController> _logger;
        private readonly ChecklistService _checklistService;

        public ServiceOrderService(IServiceOrderRepository repository, ILogger<ServiceOrderController> logger, ChecklistService checklistService)
        {
            _repository = repository;
            _logger = logger;
            _checklistService = checklistService;
        }

        public void UpdateExistingOrder(ServiceOrderModel existingOrder, ServiceOrderModel completedOrder, IFormCollection form)
        {
            // Validation
            if (existingOrder == null || completedOrder == null || form == null)
            {
                _logger.LogError("Error: existingOrder, completedOrder, or form is NULL");
                throw new ArgumentException("existingOrder, completedOrder or form cannot be NULL");
            }

            try
            {
                UpdateOrderStatus(existingOrder, "Completed");
                // UpdateCompletedAt(existingOrder, DateTime.Now);
                _checklistService.PopulateChecklistFromForm(completedOrder, form);
                UpdateChecklist(existingOrder, completedOrder.Checklists);
                // UpdateTimeToComplete(existingOrder, existingOrder.CompletedAt - existingOrder.OpenedAt);

                FieldUpdateService.UpdateFields(existingOrder, completedOrder);

                _repository.UpdateOrder(existingOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the order: {ex.Message}");
            }
        }

        public void UpdateProductName(ServiceOrderModel order, string newProductName)
        {
            order.ProductName = newProductName;
        }

        public void UpdateProductType(ServiceOrderModel order, string newProductType)
        {
            order.ProductType = newProductType;
        }

        public void UpdateWeekNumber(ServiceOrderModel order, int newWeekNumber)
        {
            order.WeekNumber = newWeekNumber;
        }

        public void UpdateDayOfWeek(ServiceOrderModel order, string newDayOfWeek)
        {
            order.DayOfWeek = newDayOfWeek;
        }

        public void UpdateOrderStatus(ServiceOrderModel order, string newOrderStatus)
        {
            order.OrderStatus = newOrderStatus;
        }

        public void UpdateContactPerson(ServiceOrderModel order, string newContactPerson)
        {
            order.ContactPerson = newContactPerson;
        }

        public void UpdateAddress(ServiceOrderModel order, string newAddress)
        {
            order.Address = newAddress;
        }

        public void UpdatePhoneNumber(ServiceOrderModel order, string newPhoneNumber)
        {
            order.PhoneNumber = newPhoneNumber;
        }

        public void UpdateEmail(ServiceOrderModel order, string newEmail)
        {
            order.Email = newEmail;
        }

        public void UpdateAgreedFinishedDate(ServiceOrderModel order, DateTime newDate)
        {
            order.AgreedFinishedDate = newDate;
        }

        public void UpdateAgreedDeliveryDate(ServiceOrderModel order, DateTime newDate)
        {
            order.AgreedDeliveryDate = newDate;
        }

        public void UpdateReceivedProductDate(ServiceOrderModel order, DateTime newDate)
        {
            order.ReceivedProductDate = newDate;
        }

        public void UpdateCompletedServiceDate(ServiceOrderModel order, DateTime newDate)
        {
            order.CompletedServiceDate = newDate;
        }

        public void UpdateHoursToComplete(ServiceOrderModel order, double newHours)
        {
            order.HoursToComplete = newHours;
        }

        public void UpdateCustomerComment(ServiceOrderModel order, string newComment)
        {
            order.CustomerComment = newComment;
        }

        public void UpdateChecklist(ServiceOrderModel order, ChecklistDto newChecklist)
        {
            order.Checklists = newChecklist;
        }

        /* public void UpdateCompletedAt(ServiceOrderModel order, DateTime newCompletedAt)
        {
            order.CompletedAt = newCompletedAt;
        } */

        public void UpdateTimeToComplete(ServiceOrderModel order, TimeSpan newTimeToComplete)
        {
            order.TimeToComplete = newTimeToComplete;
        }
    }
}