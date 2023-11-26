using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Data.Repositories;
using Noested.Models;

// This class contains unit tests for the ServiceOrderRepository class.
public class ServiceOrderRepositoryTests
{
    // This method sets up a new instance of the ServiceOrderRepository with an in-memory database context (ApplicationDbContext).
    // The in-memory database is used for testing purposes to isolate the tests from a real database.
    private static ServiceOrderRepository Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

        var context = new ApplicationDbContext(optionsBuilder.Options);

        var repo = new ServiceOrderRepository(context, null);

        return repo;
    }

    // This method creates and returns a sample ServiceOrder object with associated customer details.
    // This object is used in tests that involve adding or updating service orders.
    private static ServiceOrder AddTestOrder()
    {
        // ...
    }

    // This test checks the repository's ability to search for orders based on a criteria (in this case, the search term "John").
    // It asserts that the result contains a customer with the first name "John."
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

    // Verifies that the repository can retrieve all service orders.
    // It asserts the expected count and checks the first order's customer's first name.
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

    // Verifies that the repository can retrieve a specific order by its ID.
    // It asserts that the customer's last name of the retrieved order is "Doe."
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

    // Verifies the repository's ability to add a service order.
    // It adds a test order, retrieves all orders, and asserts the count and the description of the first order.
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

    // Verifies the repository's capability to update a service order.
    // It adds a test order, updates its description, retrieves all orders, and asserts that the description has been updated.
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

    // Verifies the repository's ability to retrieve all customers.
    // It asserts the count and checks the first names of the first, second, and last customers.
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

    // Verifies the repository's ability to retrieve a customer by their ID.
    // It asserts that the retrieved customer's first name is "Jane."
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

    // Verifies the repository's capability to add a customer.
    // It adds a customer from the test order, retrieves all customers, and asserts the count and the first name of the first customer.
    [Fact]
    public async void TestAddCustomer()
    {
        // Arrange
        var repository = Setup();

        // Act
        // Adds the customer object from the TestOrder
        await repository.AddCustomerAsync(AddTestOrder().Customer);
        var result = await repository.GetAllCustomersAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Odd", result.FirstOrDefault().FirstName);
    }
}
