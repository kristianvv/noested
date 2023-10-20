// To Remain POCO. Update methods in Services > ServiceOrderService.cs
using System;
using System.ComponentModel.DataAnnotations;
using Noested.Data;

namespace Noested.Models
{
    public class ServiceOrderModel
    {
        public ServiceOrderModel()
        {
            ServiceOrderID = 0;
            ServiceOrderStatus = "";
            OrderRecieved = DateTime.MinValue;
            AgreedFinishedDate = DateTime.MinValue;
            OrderCompleted = DateTime.MinValue;
            ProductName = "";
            ProductType = "";
            SerialNumber = 0;
            ModelYear = null;
            Warranty = WarrantyType.None;
            CustomerComment = "";
            RepairDescription = "";
            WorkHours = 0;
            Checklists = new ChecklistDTO();
            Customer = new Customer();
        }

        [Display(Name = "Serviceorder ID")] // Primary Key
        public int ServiceOrderID { get; set; }
        [Display(Name = "Serviceorder Status")]
        public string? ServiceOrderStatus { get; set; }
        [Display(Name = "Order Received")]
        public DateTime OrderRecieved { get; set; }
        [Display(Name = "Agreed Finished Date")]
        public DateTime AgreedFinishedDate { get; set; }
        [Display(Name = "Order Completed")]
        public DateTime OrderCompleted { get; set; }
        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }
        [Display(Name = "Product Type")]
        public string? ProductType { get; set; }
        [Display(Name = "Serial Number")]
        public int SerialNumber { get; set; }
        [Display(Name = "Model Year")]
        public string? ModelYear { get; set; }
        [Display(Name = "Warranty Type")]
        public WarrantyType Warranty { get; set; }
        [Display(Name = "Customer Comment")]
        public string? CustomerComment { get; set; }
        [Display(Name = "Description of Repair")]
        public string? RepairDescription { get; set; }
        [Display(Name = "Working Hours")]
        public int WorkHours { get; set; }

        // Reference to Customer
        public Customer Customer { get; set; }

        // Reference to Checklist (DTO folder)
        public ChecklistDTO Checklists { get; set; }
    }

    // Enum for Warranty
    public enum WarrantyType
    {
        None,
        Limited,
        Full
    }

    //Information about Customer
    public class Customer
    {
        public int? CustomerID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? StreetAddress { get; set; }
        public int? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
