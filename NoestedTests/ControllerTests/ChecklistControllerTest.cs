using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Noested.Controllers;
using Noested.Data;
using Noested.Models;

// This test is to make sure the checklist gets added to database. Which also
// checks the CustomersController as well to ensure it actually goes through.

namespace NoestedTests.ControllerTests
{
    public class ChecklistControllerTest
    {
        [Fact]
        public async Task Can_Add_Checklist_To_Database()
        {
            // Arrange
            var dbContext = await GetDbContext();
            var customersController = new CustomersController(dbContext);

            // Act
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Street = "123 Main St",
                PostalCode = "12345",
                City = "City",
                Email = "john.doe@example.com",
                Phone = "555-1234"
            };

            var result = await customersController.Create(customer);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CustomersController.Index), viewResult.ActionName);

            Assert.Equal(1, dbContext.Customer.Count());
            var addedCustomer = dbContext.Customer.FirstOrDefault();
            Assert.NotNull(addedCustomer);
            Assert.Equal("John", addedCustomer.FirstName);
            
            // Insert assertions based on the data model
        }

        // Ensures that the in-memory database is created before returning the context.
        
        private static async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}
