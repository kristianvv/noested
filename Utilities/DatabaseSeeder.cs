using System;
using System.Collections.Generic;
using Noested.Data;
using Noested.Models;

namespace Noested.Utilities
{

    public static class DatabaseSeeder
    {
        public static void SeedServiceOrders(ServiceOrderDatabase dbContext)
        {
            List<ServiceOrderModel> serviceOrders = new()
            {
                new ServiceOrderModel
                {
                    OrderNumber = 1,
                    ProductName = "IGLAND 5002 Pento TL",
                    ProductType = "Vinsj",
                    WeekNumber = 22,
                    DayOfWeek = "Mandag",
                    ContactPerson = "John Doe",
                    Address = "Univeien 20, 5034",
                    PhoneNumber = "85429854",
                    Email = "john.doe@gmail.com",
                    OrderStatus = "Uåpnet",
                    CustomerComment = "Motor lager ulyder"
                },
                new ServiceOrderModel
                {
                    OrderNumber = 2,
                    ProductName = "IGLAND 6002 Pronto TLP",
                    ProductType = "Vinsj",
                    WeekNumber = 23,
                    DayOfWeek = "Onsdag",
                    ContactPerson = "Jane Doe",
                    Address = "Univeien 20, 5034",
                    PhoneNumber = "45978643",
                    Email = "jane.doe@gmail.com",
                    OrderStatus = "Uåpnet",
                    CustomerComment = "Dårlig trekkraft og lukter svidd ved bruk"
                },
                new ServiceOrderModel
                {
                    OrderNumber = 3,
                    ProductName = "Igland 2501",
                    ProductType = "Vinsj",
                    WeekNumber = 24,
                    DayOfWeek = "Fredag",
                    ContactPerson = "Oluf Snøvla",
                    Address = "Birkegata 12, 5524",
                    PhoneNumber = "86543354",
                    Email = "oluf.snovla@gmail.com",
                    OrderStatus = "Uåpnet",
                    CustomerComment = "Problem med sjetting"
                }
            };

            foreach (var order in serviceOrders)
            {
                dbContext.AddServiceOrder(order);
            }
        }
    }
}