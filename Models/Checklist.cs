using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public enum ComponentStatus // Used in subclasses
    {
        Unchecked,
        OK,
        NeedsReplacement,
        Defective
    }

    public class Checklist
    {
        [Key]
        [ForeignKey("ServiceOrder")]
        public int ChecklistId { get; set; }

        public virtual ServiceOrder? ServiceOrder { get; set; }  // nav

        [Required]
        [Display(Name = "Produkttype")]
        public ProductType ProductType { get; set; }

        [Display(Name = "Utført av")]
        [MaxLength(50)]
        public string? PreparedBy { get; set; }

        [Display(Name = "Prosedyre")]
        [MaxLength(200)]
        public string? ServiceProcedure { get; set; }

        
    }
}
