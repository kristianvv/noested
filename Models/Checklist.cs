using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public enum ComponentStatus
    {
        OK,
        NeedsReplacement,
        Defective
    }
    public class Checklist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChecklistID { get; set; }

        [ForeignKey("ServiceOrder")]
        [Required]
        [Display(Name = "Ordrenummer")]
        public int OrderID { get; set; }

        [Required]
        [Display(Name = "Prosedyre")]
        [MaxLength(200)]
        public string? ServiceProcedure { get; set; }

        [Required]
        [Display(Name = "Godkjent av")]
        [MaxLength(50)]
        public string? ApprovedBy { get; set; }

        [Required]
        [Display(Name = "Utført av")]
        [MaxLength(50)]
        public string? PreparedBy { get; set; }

        [Required]
        [Display(Name = "Bremser")]
        public ComponentStatus MechBrakes { get; set; }

        [Required]
        [Display(Name = "Trommellager")]
        public ComponentStatus MechDrumBearing { get; set; }

        [Required]
        [Display(Name = "PTO & opplagring")]
        public ComponentStatus MechStoragePTO { get; set; }

        [Required]
        [Display(Name = "Wire")]
        public ComponentStatus MechWire { get; set; }

        [Required]
        [Display(Name = "Kjedestrammer")]
        public ComponentStatus MechChainTensioner { get; set; }

        [Required]
        [Display(Name = "Pinionlager")]
        public ComponentStatus MechPinionBearing { get; set; }

        [Required]
        [Display(Name = "Clutch")]
        public ComponentStatus MechClutch { get; set; }

        [Required]
        [Display(Name = "Kjedehjulkile")]
        public ComponentStatus MechSprocketWedges { get; set; }

        [Required]
        [Display(Name = "Hydraulikksylinder")]
        public ComponentStatus HydCylinder { get; set; }

        [Required]
        [Display(Name = "Hydraulikkblokk")]
        public ComponentStatus HydHydraulicBlock { get; set; }

        [Required]
        [Display(Name = "Tankolje")]
        public ComponentStatus HydTankOil { get; set; }

        [Required]
        [Display(Name = "Girboksolje")]
        public ComponentStatus HydGearboxOil { get; set; }

        [Required]
        [Display(Name = "Bremsesylinder")]
        public ComponentStatus HydBrakeCylinder { get; set; }

        [Required]
        [Display(Name = "Ledningsnett")]
        public ComponentStatus ElCableNetwork { get; set; }

        [Required]
        [Display(Name = "Radio")]
        public ComponentStatus ElRadio { get; set; }

        [Required]
        [Display(Name = "Knappekasse")]
        public ComponentStatus ElButtonBox { get; set; }

        [Required]
        [Display(Name = "Bar")]
        public ComponentStatus TensionCheckBar { get; set; }

        [Required]
        [Display(Name = "Vinsjtest")]
        public ComponentStatus TestWinch { get; set; }

        [Required]
        [Display(Name = "Trekkraft")]
        public ComponentStatus TestTraction { get; set; }

        [Display(Name = "Kommentar")]
        [MaxLength(200)]
        public string? Comment { get; set; }

        [Required]
        [Display(Name = "Signatur")]
        [MaxLength(50)]
        public string? MechSignature { get; set; }

        [Required]
        [Display(Name = "Dato fullført")]
        public DateTime DateCompleted { get; set; }
    }
}
