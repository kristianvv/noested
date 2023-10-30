using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noested.Models
{
    public class ServiceOrder
    {
        public enum OrderStatus
        {
            Recieved,
            InProgress,
            Completed
        }

        public enum WarrantyType
        {
            None,
            Limited,
            Full
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Ordrenr")]
        public int OrderId { get; set; }

        [ForeignKey("CustomerId")]
        [Display(Name = "Kundenr")]
        public int CustomerId { get; set; }

        public virtual Customer? Customer { get; set; } // nav

        [ForeignKey("ChecklistId")]
        [Display(Name = "Sjekklistenr")]
        public int? ChecklistId { get; set; }

        public virtual Checklist? Checklist { get; set; } // nav
        //
        [Required]
        [Display(Name = "Aktiv")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Mottatt")]
        public DateTime OrderReceived { get; set; }

        [Display(Name = "Fullført")]
        public DateTime? OrderCompleted { get; set; }

        [Required]
        [Display(Name = "Ordrestatus")]
        public OrderStatus Status { get; set; }

        [Display(Name = "Agreed Finished Date")]
        public DateTime? AgreedFinishedDate { get; set; }

        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public string? ProductType { get; set; }

        [MaxLength(4)]
        [Display(Name = "Årsmodell")]
        public string? ModelYear { get; set; }

        [MaxLength(50)]
        [Display(Name = "Serienummer")]
        public int SerialNumber { get; set; }

        [Required]
        [Display(Name = "Garanti")]
        public WarrantyType Warranty { get; set; }

        [MaxLength(200)]
        [Display(Name = "Kundeavtale")]
        public string? CustomerAgreement { get; set; }

        [MaxLength(200)]
        [Display(Name = "Beskrivelse")]
        public string? OrderDescription { get; set; }

        [MaxLength(200)]
        [Display(Name = "Avlagte deler")]
        public string? DiscardedParts { get; set; }

        [MaxLength(200)]
        [Display(Name = "Deleretur")]
        public string? ReplacedPartsReturned { get; set; }

        [MaxLength(50)]
        [Display(Name = "Frakt")]
        public string? Shipping { get; set; }

        [Display(Name = "Working Hours")]
        public int WorkHours { get; set; }
    }
}

