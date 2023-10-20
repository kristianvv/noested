using Noested.Data;
using Noested.Models;
using Noested.Models.DTOs;

namespace Noested.Services
{
    public class ServiceOrderService
    {
        private readonly IServiceOrderRepository _repository;

        public ServiceOrderService(IServiceOrderRepository repository)
        {
            _repository = repository;
        }

        //
        public async Task UpdateExistingOrderAsync(ServiceOrderModel existingOrder, ServiceOrderModel completedOrder, IFormCollection? form)
        {
            await UpdateOrderStatusAsync(existingOrder, "Completed");
            await UpdateOrderCompletedAsync(existingOrder, DateTime.Now);
            await ChecklistService.PopulateChecklistFromForm(completedOrder, form);
            await UpdateChecklistsAsync(existingOrder, completedOrder.Checklists);
            await FieldUpdateService.UpdateFieldsAsync(existingOrder, completedOrder);
            await _repository.UpdateOrderAsync(existingOrder);
        }

        //
        public async Task UpdateServiceOrderIDAsync(ServiceOrderModel order, int newServiceOrderID)
        {
            order.ServiceOrderID = newServiceOrderID;
            await Task.CompletedTask;
        }

        public async Task UpdateOrderStatusAsync(ServiceOrderModel order, string newOrderStatus)
        {
            order.ServiceOrderStatus = newOrderStatus;
            await Task.CompletedTask;
        }

        public async Task UpdateOrderRecievedAsync(ServiceOrderModel order, DateTime newOrderRecieved)
        {
            order.OrderRecieved = newOrderRecieved;
            await Task.CompletedTask;
        }

        public async Task UpdateOrderCompletedAsync(ServiceOrderModel order, DateTime newOrderCompleted)
        {
            order.OrderCompleted = newOrderCompleted;
            await Task.CompletedTask;
        }

        public async Task UpdateSerialNumberAsync(ServiceOrderModel order, int newSerialNumber)
        {
            order.SerialNumber = newSerialNumber;
            await Task.CompletedTask;
        }

        public async Task UpdateModelYearAsync(ServiceOrderModel order, string newModelYear)
        {
            order.ModelYear = newModelYear;
            await Task.CompletedTask;
        }

        public async Task UpdateWarrantyAsync(ServiceOrderModel order, WarrantyType newWarranty)
        {
            order.Warranty = newWarranty;
            await Task.CompletedTask;
        }

        public async Task UpdateRepairDescriptionAsync(ServiceOrderModel order, string newRepairDescription)
        {
            order.RepairDescription = newRepairDescription;
            await Task.CompletedTask;
        }

        public async Task UpdateWorkHoursAsync(ServiceOrderModel order, int newWorkHours)
        {
            order.WorkHours = newWorkHours;
            await Task.CompletedTask;
        }

        public async Task UpdateCustomerAsync(ServiceOrderModel order, Customer newCustomer)
        {
            order.Customer = newCustomer;
            await Task.CompletedTask;
        }

        public async Task UpdateCustomerIDAsync(ServiceOrderModel order, int newCustomerID)
        {
            order.Customer!.CustomerID = newCustomerID;
            await Task.CompletedTask;
        }

        public async Task UpdateChecklistsAsync(ServiceOrderModel order, ChecklistDTO newChecklists)
        {
            order.Checklists = newChecklists;
            await Task.CompletedTask;
        }
    }
}