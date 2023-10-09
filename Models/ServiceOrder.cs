using System.ComponentModel.DataAnnotations.Schema;

namespace Noested.Models
{
    public class ServiceOrder
    {
        public int ServiceOrderID { get; set; }
        public DateTime OrderRecieved { get; set; }
        public DateTime OrderCompleted { get; set; }
        public int SerialNumber { get; set; }
        public string ModelYear { get; set; }
        public WarrantyType Warranty { get; set; }
        public string CustomerAgreement { get; set; }
        public string RepairDescription { get; set; }
        public string ExpiredParts { get; set; }
        public int WorkHours { get; set; }
        public string ReplacedPartsReturned { get; set; }
        public string ShippingMethod { get; set; }

        // Reference to Customer
        public Customer Customer { get; set; }

        // Foreign key to Customer
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

    }

    // Enum for Warranty
    public enum WarrantyType
    {
        None,
        Limited,
        Full
    }

    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
