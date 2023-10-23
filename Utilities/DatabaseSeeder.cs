using Noested.Data;
using Noested.Models;
using Noested.Services;

namespace Noested.Utilities
{
    public static class DatabaseSeeder
    {
        public static void SeedServiceOrders(IServiceOrderRepository dbContext)
        {
            List<ServiceOrderModel> serviceOrders = new()
            {
                new ServiceOrderModel
                {
                    ServiceOrderID = 1,
                    CustomerID = 1,
                    ServiceOrderStatus = "Uåpnet",
                    OrderReceived = DateTime.Now,
                    AgreedFinishedDate = DateTime.MinValue,
                    ProductName = "IGLAND 5002 Pento TL",
                    ProductType = "Vinsj",
                    SerialNumber = 12345,
                    ModelYear = "2022",
                    Warranty = WarrantyType.Full,
                    CustomerComment = "Problem with motor",
                    Checklists = new ChecklistDTO(),
                    Customer = new Customer
                    {
                        CustomerID = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        StreetAddress = "Univeien 20",
                        ZipCode = 5034,
                        City = "Cityville",
                        Email = "john.doe@gmail.com",
                        Phone = "85429854"
                    }
                },
                new ServiceOrderModel
                {
                    ServiceOrderID = 2,
                    CustomerID = 2,
                    ServiceOrderStatus = "Uåpnet",
                    OrderReceived = DateTime.Now,
                    AgreedFinishedDate = DateTime.MinValue,
                    ProductName = "IGLAND 6002 Pronto TLP",
                    ProductType = "Vinsj",
                    SerialNumber = 54321,
                    ModelYear = "2023",
                    Warranty = WarrantyType.Limited,
                    CustomerComment = "Problem with traction",
                    Checklists = new ChecklistDTO(),
                    Customer = new Customer
                    {
                        CustomerID = 2,
                        FirstName = "Jane",
                        LastName = "Doe",
                        StreetAddress = "Univeien 20",
                        ZipCode = 5034,
                        City = "Cityville",
                        Email = "jane.doe@gmail.com",
                        Phone = "45978643"
                    }
                },
                new ServiceOrderModel
                {
                    ServiceOrderID = 3,
                    CustomerID = 3,
                    ServiceOrderStatus = "Uåpnet",
                    OrderReceived = DateTime.Now,
                    AgreedFinishedDate = DateTime.MinValue,
                    ProductName = "Igland 2501",
                    ProductType = "Vinsj",
                    SerialNumber = 98765,
                    ModelYear = "2021",
                    Warranty = WarrantyType.None,
                    CustomerComment = "Ulyder i motoren og svidd lukt",
                    Checklists = new ChecklistDTO(),
                    Customer = new Customer
                    {
                        CustomerID = 3,
                        FirstName = "Oluf",
                        LastName = "Snøvla",
                        StreetAddress = "Birkegata 12",
                        ZipCode = 5524,
                        City = "Snowtown",
                        Email = "oluf.snovla@gmail.com",
                        Phone = "86543354"
                    }
                }
            };
            // Legger ordrene til databasen
            foreach (ServiceOrderModel order in serviceOrders)
            {
                dbContext.AddServiceOrderAsync(order);
            }

            // Legger kundeinfo fra ServiceOrdrene inn i egen liste i db
            List<Customer> existingCustomers = dbContext.GetAllCustomersAsync().Result.ToList();
            foreach (ServiceOrderModel order in serviceOrders)
            {
                Customer customer = order.Customer;

                // Validerer at CustomerID and Email er unike
                if (existingCustomers != null && customer != null)
                {
                    if (existingCustomers.Any(c => c != null && (c.CustomerID == customer.CustomerID || c.Email == customer.Email)))
                    {
                        // logger feil, kunne  ikke bruke Ilogger her.
                        Console.WriteLine($"Warning: Duplicate CustomerID or Email found for CustomerID: {customer.CustomerID}, Email: {customer.Email}");
                    }
                    else
                    {
                        dbContext.AddCustomerAsync(customer); // Oppdaterer listen med kunder.  
                    }
                }

            }
        }
    }
}