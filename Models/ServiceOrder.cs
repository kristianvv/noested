using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public enum OrderStatus
    {
        Recieved,
        InProgress,
        Completed
    }

    public enum WarrantyStatus
    {
        Full,
        Limited,
        None
    }
    public class ServiceOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Ordrenr")]
        public int OrderID { get; set; }

        [ForeignKey("Customer")]
        [Required]
        [Display(Name = "Kundenr")]
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "Aktiv")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Mottatt")]
        public DateTime OrderRecieved { get; set; }

        [Display(Name = "Fullført")]
        public DateTime? OrderCompleted { get; set; }

        [Required]
        [Display(Name = "Ordrestatus")]
        public OrderStatus OrderStatus { get; set; }

        [Required]
        [MaxLength(4)]
        [Display(Name = "Årsmodell")]
        public string? ModelYear { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Serienummer")]
        public string? SerialNumber { get; set; }

        [Required]
        [Display(Name = "Garanti")]
        public WarrantyStatus Warranty { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Kundeavtale")]
        public string? CustomerAgreement { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Beskrivelse")]
        public string? OrderDescription { get; set; }

        [MaxLength(200)]
        [Display(Name = "Avlagte deler")]
        public string? DiscardedParts { get; set; }

        [MaxLength(200)]
        [Display(Name = "Deleretur")]
        public string? ReplacedPartsReturned { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Frakt")]
        public string? Shipping { get; set; }
    }

}

