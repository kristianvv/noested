using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Models;

public static class DatabaseSeeder
{
    public static async Task SeedServiceOrders(IServiceOrderRepository dbContext)
    {
        try
        {
            List<ServiceOrder> serviceOrders = new()
            {
                new ServiceOrder
                {
                    OrderId = 1,
                    CustomerId = 1,
                    IsActive = true,
                    OrderReceived = DateTime.Now,
                    OrderCompleted = null,
                    Status = ServiceOrder.OrderStatus.Recieved,
                    AgreedFinishedDate = DateTime.Now.AddDays(7),
                    ProductName = "IGLAND 5002 Pento TL",
                    ProductType = "Vinsj",
                    ModelYear = "2022",
                    SerialNumber = 12345,
                    Warranty = ServiceOrder.WarrantyType.Full,
                    CustomerAgreement = "Standard Agreement",
                    OrderDescription = "Problem with motor",
                    DiscardedParts = "",
                    ReplacedPartsReturned = "",
                    Shipping = "",
                    WorkHours = 0,
                    Customer = new Customer
                    {
                        CustomerId = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        Street = "Univeien 20",
                        PostalCode = "5034",
                        City = "Cityville",
                        Email = "john.doe@gmail.com",
                        Phone = "85429854"
                    }
                },
                new ServiceOrder
                {
                    IsActive = true,
                    OrderReceived = DateTime.Now,
                    OrderCompleted = null,
                    Status = ServiceOrder.OrderStatus.Recieved,
                    AgreedFinishedDate = DateTime.Now.AddDays(7),
                    ProductName = "IGLAND 6002 Pento TL",
                    ProductType = "Vinsj",
                    ModelYear = "2021",
                    SerialNumber = 67890,
                    Warranty = ServiceOrder.WarrantyType.Limited,
                    CustomerAgreement = "Premium Agreement",
                    OrderDescription = "Problem with cable",
                    DiscardedParts = "",
                    ReplacedPartsReturned = "",
                    Shipping = "",
                    WorkHours = 0,
                    Customer = new Customer
                    {
                        FirstName = "Jane",
                        LastName = "Doe",
                        Street = "Univeien 21",
                        PostalCode = "5035",
                        City = "Cityville",
                        Email = "jane.doe@gmail.com",
                        Phone = "85429855"
                    }
                },
                new ServiceOrder
                {
                    IsActive = true,
                    OrderReceived = DateTime.Now,
                    OrderCompleted = null,
                    Status = ServiceOrder.OrderStatus.Recieved,
                    AgreedFinishedDate = DateTime.Now.AddDays(7),
                    ProductName = "IGLAND 7002 Pento TL",
                    ProductType = "Vinsj",
                    ModelYear = "2020",
                    SerialNumber = 11111,
                    Warranty = ServiceOrder.WarrantyType.None,
                    CustomerAgreement = "Basic Agreement",
                    OrderDescription = "Problem with remote",
                    DiscardedParts = "",
                    ReplacedPartsReturned = "",
                    Shipping = "",
                    WorkHours = 0,
                    Customer = new Customer
                    {
                        FirstName = "Emily",
                        LastName = "Smith",
                        Street = "Univeien 22",
                        PostalCode = "5036",
                        City = "Cityville",
                        Email = "emily.smith@gmail.com",
                        Phone = "85429856"
                    }
                }
            };

            // Adding orders to the database
            foreach (ServiceOrder order in serviceOrders)
            {
                await dbContext.AddServiceOrderAsync(order);
                await dbContext.AddCustomerAsync(order.Customer!);
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
        }
    }
}
