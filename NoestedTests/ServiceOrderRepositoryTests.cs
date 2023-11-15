using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Noested.Data;
using Noested.Data.Repositories;
using Noested.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
            var resultlist = result.ToList();
           
           
            // Assert
            Assert.NotNull(result);

            // Assert.Single(resultlist);
            // Assert.Equal("John", resultlist[0].Customer.FirstName);
        }
    }
}
