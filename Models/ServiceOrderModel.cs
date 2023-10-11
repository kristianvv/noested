// To Remain POCO. Update methods in Services > ServiceOrderService.cs
using System.ComponentModel.DataAnnotations;
using Noested.Data.DTOs;
using Noested.Models.DTOs;

namespace Noested.Models
{
    public class ServiceOrderModel
    {
        public ServiceOrderModel()
        {
            OrderNumber = 0;
            ProductName = "";
            ProductType = "";
            WeekNumber = 0;
            DayOfWeek = "";
            OrderStatus = "";
            ContactPerson = "";
            Address = "";
            PhoneNumber = "";
            Email = "";
            CustomerComment = "";
            MechanicCommentary = "";
            Signature = "";
            AgreedFinishedDate = DateTime.Now;
            AgreedDeliveryDate = DateTime.Now;
            ReceivedProductDate = DateTime.Now;
            CompletedServiceDate = DateTime.Now;
            HoursToComplete = 0.0;
            OpenedAt = DateTime.Now;
            TimeToOpen = new TimeSpan(0);
            CompletedAt = DateTime.Now;
            TimeToComplete = new TimeSpan(0);

            Checklists = new ChecklistDto();
        }
        // DISPLAYED ON SERIVCE ORDER TICKETS IN MECH VIEW (Views > ServiceOrder > Index.cshtml)
        [Required]
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Product Type")]
        public string ProductType { get; set; }

        [Required]
        [Display(Name = "Week Number")]
        public int WeekNumber { get; set; }

        [Required]
        [Display(Name = "Day of the Week")]
        public string DayOfWeek { get; set; }

        [Required]
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }

        // DISPLAYED AFTER OPENING SERVICE ORDER
        // Left Section
        [Required]
        [Display(Name = "Customer's Name or Company")]
        public string ContactPerson { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone/Mobile Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        // Right Section
        public DateTime AgreedFinishedDate { get; set; }
        public DateTime AgreedDeliveryDate { get; set; }
        public DateTime ReceivedProductDate { get; set; }
        public DateTime CompletedServiceDate { get; set; }
        public double HoursToComplete { get; set; }

        // ViewOrder.cshtml: Under Left- and Right Sections
        [Required]
        [Display(Name = "Customer's Description of Problem")]
        public string CustomerComment { get; set; }

        // Checklist Items Radio Options (Under Customer's Description)
        public ChecklistDto Checklists { get; set; }

        // Mechanics Commentary and Signature
        [Required]
        [Display(Name = "Mechanics Comments")]
        public string MechanicCommentary { get; set; }

        [Required]
        [Display(Name = "Mechanics Signature")]
        public string Signature { get; set; }

        // ACTION INFORMATION ON SERVICE ORDERS (Invisible & for generating reports only)
        public DateTime OpenedAt { get; set; } // When was it opened
        public TimeSpan TimeToOpen { get; set; } // Difference between creating it and opening it
        public DateTime CompletedAt { get; set; } // When was it finished (checklist submit)
        public TimeSpan TimeToComplete { get; set; } // Difference between opening and submitting.
    }
}
