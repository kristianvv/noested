using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Data.Repositories;
using Noested.Models;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;

namespace NoestedTests
{
    public class ServiceOrderRepositoryTests
    {
        private static ServiceOrderRepository Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString());
            
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var repo = new ServiceOrderRepository(context, null);

            return repo;
                
        }

        [Fact]
        public async void SearchTest()
        {
            // Arrange
            var repository = Setup();
            DatabaseSeeder.SeedServiceOrders(repository).Wait();

            // Act
            var result = await repository.Search("John");
            var resultlist = result.ToList();

            // Assert
            Assert.NotNull(result);

            Assert.Single(resultlist);

            Assert.Equal("John", resultlist[0].Customer.FirstName);

        }

    }       
}
