using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Data.Repositories;

namespace NoestedTests
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
    }
}
