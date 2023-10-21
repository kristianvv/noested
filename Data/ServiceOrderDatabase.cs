using System.Collections.Generic;
using Noested.Models;
using Noested.Models.DTOs;

namespace Noested.Data
{
    public class ServiceOrderDatabase
    {
        private List<ServiceOrderModel> ServiceOrders { get; set; } = new List<ServiceOrderModel>();
        private List<ChecklistDTO> ChecklistsDto { get; set; } = new List<ChecklistDTO>();
        private List<Customer> Customers { get; set; } = new List<Customer>();
        private int LastOrderNumber { get; set; } = 0;
        private int LastCustomerID { get; set; } = 0;

        public required ISet<User> Users { get; set; } = new HashSet<User>();// For testing purposes (From AppDbContext.cs – To be deleted?)
        public List<Test> Test { get; set; } = new List<Test>(); // (From NoestedContext.cs - To be deleted?)
        public ISet<ServiceOrderModel>? ServiceOrder { get; set; } = new HashSet<ServiceOrderModel>(); // (From NoestedContext.cs - To be deleted?)
        public ISet<DummyServiceOrder> DummyServiceOrder { get; set; } = new HashSet<DummyServiceOrder>(); // (From NoestedContext.cs - To be deleted?)

        // For Customers: Add
        public Task AddCustomerAsync(Customer newCustomer)
        {
            LastCustomerID++; // Increment the last CustomerID
            newCustomer.CustomerID = LastCustomerID; // Set the increment as new ID.
            Customers.Add(newCustomer);
            return Task.FromResult(0);
        }

        // GetAll
        public Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return Task.FromResult<IEnumerable<Customer>>(Customers);
        }

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
            LastOrderNumber++; // Increment the last ServiceOrderID
            order.OrderRecieved = DateTime.Now;
            order.ServiceOrderID = LastOrderNumber; // Set the increment as new ID
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

        




        // TEST METHODS (Controller: "TestsController.cs". Model: "Test.cs". View-folder: "Tests" > Create, Delete, Details, Edit)

        // Create
        public Task AddTestAsync(Test test)
        {
            Test.Add(test);
            return Task.FromResult(0);
        }
        // Delete
        public Task DeleteTestAsync(int id)
        {
            var test = Test.Find(t => t.Id == id);
            if (test != null)
            {
                Test.Remove(test);
            }
            return Task.FromResult(0);
        }
        // Details
        public Task<Test?> GetTestByIdAsync(int id)
        {
            var result = Test.Find(t => t.Id == id);
            return Task.FromResult(result);
        }
        // Edit
        public Task UpdateTestAsync(Test updatedTest)
        {
            var existingTest = Test.Find(t => t.Id == updatedTest.Id);
            if (existingTest != null)
            {
                existingTest.Title = updatedTest.Title;
                existingTest.ReleaseDate = updatedTest.ReleaseDate;
                existingTest.Genre = updatedTest.Genre;
                existingTest.Price = updatedTest.Price;
            }
            return Task.FromResult(0);
        }
        // GET ALL (Index)
        public Task<IEnumerable<Test>> GetAllTestsAsync()
        {
            return Task.FromResult<IEnumerable<Test>>(Test);
        }

    }
}

