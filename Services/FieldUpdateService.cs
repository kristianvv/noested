using Noested.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Noested.Services
{
    public class FieldUpdateService
    {


        public static async Task UpdateFieldsAsync(ServiceOrderModel existingOrder, ServiceOrderModel completedOrder, IFormCollection? form)
        {

            if (existingOrder == null)
            {
                throw new ArgumentNullException(nameof(existingOrder), "existingOrder is null");
            }

            if (completedOrder == null)
            {
                throw new ArgumentNullException(nameof(completedOrder), "completedOrder is null");
            }

            if (form == null)
            {
                throw new ArgumentNullException(nameof(form), "form is null");
            }


            // Validate and update individual fields
            await ValidateAndUpdateFields(existingOrder, completedOrder, form);

            await Task.CompletedTask;
        }

        private static async Task ValidateAndUpdateFields(ServiceOrderModel existingOrder, ServiceOrderModel completedOrder, IFormCollection form)
        {
            // Validate and update ServiceOrderID
            if (completedOrder.ServiceOrderID != 0)
            {
                existingOrder.ServiceOrderID = completedOrder.ServiceOrderID;
            }

            // Validate and update ServiceOrderStatus
            if (!string.IsNullOrEmpty(completedOrder.ServiceOrderStatus))
            {
                existingOrder.ServiceOrderStatus = completedOrder.ServiceOrderStatus;
            }

            // Validate and update OrderRecieved
            if (completedOrder.OrderReceived != DateTime.MinValue)
            {
                existingOrder.OrderReceived = completedOrder.OrderReceived;
            }

            // Validate and update OrderCompleted
            if (completedOrder.OrderCompleted != DateTime.MinValue)
            {
                existingOrder.OrderCompleted = completedOrder.OrderCompleted;
            }

            // Validate and update SerialNumber
            if (completedOrder.SerialNumber != 0)
            {
                existingOrder.SerialNumber = completedOrder.SerialNumber;
            }

            // Validate and update ModelYear
            if (!string.IsNullOrEmpty(completedOrder.ModelYear))
            {
                existingOrder.ModelYear = completedOrder.ModelYear;
            }

            // Validate and update Warranty
            if (completedOrder.Warranty != WarrantyType.None)
            {
                existingOrder.Warranty = completedOrder.Warranty;
            }

            // Validate and update RepairDescription
            if (!string.IsNullOrEmpty(completedOrder.RepairDescription))
            {
                existingOrder.RepairDescription = completedOrder.RepairDescription;
            }

            // Validate and update WorkHours
            if (completedOrder.WorkHours != 0)
            {
                existingOrder.WorkHours = completedOrder.WorkHours;
            }

            // Validate and update Customer
            if (completedOrder.Customer != null && completedOrder.Customer.CustomerID != 0)
            {
                existingOrder.Customer = completedOrder.Customer;
            }

            // Validate and update Checklists
            if (form != null)
            {
                await ChecklistService.PopulateChecklistFromForm(completedOrder, form);
                existingOrder.Checklists = completedOrder.Checklists;
            }
        }
    }
}

