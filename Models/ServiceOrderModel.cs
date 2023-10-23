// To Remain POCO. Update methods in Services > ServiceOrderService.cs
using Noested.Services;
using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public class ServiceOrderModel
    {
        public ServiceOrderModel()
        {
            ServiceOrderID = 0; // is on form
            CustomerID = 0;
            ServiceOrderStatus = ""; // is on form
            OrderReceived = DateTime.MinValue; // is on form
            AgreedFinishedDate = DateTime.MinValue; // is on form hidden
            OrderCompleted = DateTime.MinValue; // is on form hidden
            ProductName = ""; // is on form
            ProductType = ""; // is on form
            SerialNumber = 0; // is on form hidden
            ModelYear = null; // is on form hidden
            Warranty = WarrantyType.None; // is on form hidden
            CustomerComment = ""; // is on form hidden
            RepairDescription = ""; // is on form
            WorkHours = 0; // is on form hidden
            Checklists = new ChecklistDTO();
            Customer = new Customer();
        }

        [Display(Name = "Serviceorder ID")] 
        public int ServiceOrderID { get; set; } // Primary Key
        [Display(Name = "Customer ID")] 
        public int CustomerID { get; set; } // Foreign Key
        [Display(Name = "Serviceorder Status")]
        public string ServiceOrderStatus { get; set; }
        [Display(Name = "Order Received")]
        public DateTime OrderReceived { get; set; }
        [Display(Name = "Agreed Finished Date")]
        public DateTime? AgreedFinishedDate { get; set; }
        [Display(Name = "Order Completed")]
        public DateTime? OrderCompleted { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Type")]
        public string ProductType { get; set; }
        [Display(Name = "Serial Number")]
        public int SerialNumber { get; set; }
        [Display(Name = "Model Year")]
        public string? ModelYear { get; set; }
        [Display(Name = "Warranty Type")]
        public WarrantyType Warranty { get; set; }
        [Display(Name = "Customer Comment")]
        public string CustomerComment { get; set; }
        [Display(Name = "Description of Repair")]
        public string RepairDescription { get; set; }
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
        public Customer()
        {
            CustomerID = 0;
            FirstName = "";
            LastName = "";
            StreetAddress = "";
            ZipCode = 0;
            City = "";
            Email = "";
            Phone = "";
        }
        public int CustomerID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? StreetAddress { get; set; }
        public int? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

}