using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Models;

namespace NoestedTests.RepositoryTests
{
    public class ServiceOrderRepositoryTests
    {
        private static ServiceOrderRepository Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            var context = new ApplicationDbContext(optionsBuilder.Options);

            var repo = new ServiceOrderRepository(context, null);

            return repo;
        }

        private static ServiceOrder AddTestOrder()
        {
            return new ServiceOrder
            {
                IsActive = true,
                OrderReceived = DateTime.Now,
                OrderCompleted = null,
                Status = OrderStatus.Received,
                AgreedFinishedDate = DateTime.Now.AddDays(7),
                ProductName = "IGLAND 5002 Pento TL",
                Product = ProductType.Winch,
                ModelYear = "2022",
                SerialNumber = "12345",
                Warranty = WarrantyType.Full,
                CustomerAgreement = "Standard Agreement",
                OrderDescription = "Problem with motor",
                DiscardedParts = "",
                ReplacedPartsReturned = "",
                Shipping = "",
                WorkHours = 0,
                Customer = new Customer
                {
                    FirstName = "Odd",
                    LastName = "Hansen",
                    Street = "Kaserneveien 30",
                    PostalCode = "4631",
                    City = "By",
                    Email = "",
                    Phone = "85429854"
                }
            };
        }

        [Fact]
        public async Task SearchTest()
        {
            // Arrange
            var repository = Setup();
            await DatabaseSeeder.SeedServiceOrders(repository);

            // Act
            var result = await repository.Search("John");

            // Assert

            Assert.Equal("John", result.FirstOrDefault().Customer.FirstName);
        }

        [Fact]
        public async Task TestGetAllServiceOrders()
        {
            // Arrange
            var repository = Setup();
            await DatabaseSeeder.SeedServiceOrders(repository);

            // Act
            var result = await repository.GetAllServiceOrdersAsync();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("John", result.FirstOrDefault().Customer.FirstName);
        }

        [Fact]
        public async Task TestGetOrderById()
        {
            // Arrange
            var repository = Setup();
            await DatabaseSeeder.SeedServiceOrders(repository);

            // Act
            var result = await repository.GetOrderByIdAsync(2);

            // Assert
            Assert.Equal("Doe", result.Customer.LastName);
        }

        [Fact]
        public async void TestAddServiceOrder()
        {
            // Arrange
            var repository = Setup();

            // Act
            await repository.AddServiceOrderAsync(AddTestOrder());
            var result = await repository.GetAllServiceOrdersAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Problem with motor", result.FirstOrDefault().OrderDescription);
        }

        [Fact]
        public async void TestUpdateServiceOrder()
        {
            // Arrange
            var repository = Setup();
            await repository.AddServiceOrderAsync(AddTestOrder());

            var result = await repository.GetAllServiceOrdersAsync();

            var orderToUpdate = result.FirstOrDefault();
            orderToUpdate.OrderDescription = "Motor repaired";

            // Act
            await repository.UpdateOrderAsync(orderToUpdate);
            var updatedResult = await repository.GetAllServiceOrdersAsync();

            // Assert
            Assert.Equal("Motor repaired", updatedResult.FirstOrDefault().OrderDescription);
        }

        [Fact]
        public async void TestGetAllCustomers()
        {

            // Arrange
            var repository = Setup();
            await DatabaseSeeder.SeedServiceOrders(repository);

            // Act
            var result = await repository.GetAllCustomersAsync();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("John", result.FirstOrDefault().FirstName);
            Assert.Equal("Jane", result.Skip(1).FirstOrDefault().FirstName);
            Assert.Equal("Emily", result.LastOrDefault().FirstName);
        }

        [Fact]
        public async void TestGetCustomerById()
        {
            // Arrange
            var repository = Setup();
            await DatabaseSeeder.SeedServiceOrders(repository);

            // Act
            var result = await repository.GetCustomerByIdAsync(2);

            // Assert
            Assert.Equal("Jane", result.FirstName);
        }

        [Fact]
        public async void TestAddCustomer()
        {
            // Arrange
            var repository = Setup();

            // Act
            await repository.AddCustomerAsync(AddTestOrder().Customer); //Legger til kunde-objektet fra testordren
            var result = await repository.GetAllCustomersAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Odd", result.FirstOrDefault().FirstName);
        }
    }
}
