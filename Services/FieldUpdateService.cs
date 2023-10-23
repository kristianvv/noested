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


            // Validere og oppdatere individuelle felt
            await ValidateAndUpdateFields(existingOrder, completedOrder, form);

            await Task.CompletedTask;
        }

        private static async Task ValidateAndUpdateFields(ServiceOrderModel existingOrder, ServiceOrderModel completedOrder, IFormCollection form)
        {
            // Validere og oppdatere ServiceOrderID
            if (completedOrder.ServiceOrderID != 0)
            {
                existingOrder.ServiceOrderID = completedOrder.ServiceOrderID;
            }

            // Validere og oppdatere ServiceOrderStatus
            if (!string.IsNullOrEmpty(completedOrder.ServiceOrderStatus))
            {
                existingOrder.ServiceOrderStatus = completedOrder.ServiceOrderStatus;
            }

            // Validere og oppdatere OrderRecieved
            if (completedOrder.OrderReceived != DateTime.MinValue)
            {
                existingOrder.OrderReceived = completedOrder.OrderReceived;
            }

            // Validere og oppdatere OrderCompleted
            if (completedOrder.OrderCompleted != DateTime.MinValue)
            {
                existingOrder.OrderCompleted = completedOrder.OrderCompleted;
            }

            // Validere og oppdatere SerialNumber
            if (completedOrder.SerialNumber != 0)
            {
                existingOrder.SerialNumber = completedOrder.SerialNumber;
            }

            // Validere og oppdatere ModelYear
            if (!string.IsNullOrEmpty(completedOrder.ModelYear))
            {
                existingOrder.ModelYear = completedOrder.ModelYear;
            }

            // Validere og oppdatere Warranty
            if (completedOrder.Warranty != WarrantyType.None)
            {
                existingOrder.Warranty = completedOrder.Warranty;
            }

            // Validere og oppdatere RepairDescription
            if (!string.IsNullOrEmpty(completedOrder.RepairDescription))
            {
                existingOrder.RepairDescription = completedOrder.RepairDescription;
            }

            // Validere og oppdatere WorkHours
            if (completedOrder.WorkHours != 0)
            {
                existingOrder.WorkHours = completedOrder.WorkHours;
            }

            // Validere og oppdatere Customer
            if (completedOrder.Customer != null && completedOrder.Customer.CustomerID != 0)
            {
                existingOrder.Customer = completedOrder.Customer;
            }

            // Validere og oppdatere Checklists
            if (form != null)
            {
                await ChecklistService.PopulateChecklistFromForm(completedOrder, form);
                existingOrder.Checklists = completedOrder.Checklists;
            }
        }
    }
}

