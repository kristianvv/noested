using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noested.Models
{
    [Table("LiftEquip")] // Specify the table name for the subclass
    public class LiftEquip : Checklist
    {
        [Required]
        [Display(Name = "Test")]
        public ComponentStatus Test { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Test2")]
        public ComponentStatus Test2 { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Test3")]
        public ComponentStatus Test3 { get; set; } = ComponentStatus.Unchecked;

       
    }
}