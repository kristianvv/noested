using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public class Customer
    {
        [Key] // Setter primærnøkkel
        [Display(Name = "Kundenr")]
        public int CustomerId { get; set; }
        [Required]
        [Display(Name = "Fornavn")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Etternavn")]
        public string? LastName { get; set; }
        [Required]
        [Display(Name = "Adresse")]
        public string? Street { get; set; }
        [Required]
        [Display(Name = "Postnummer")]
        public string? PostalCode { get; set; }
        [Required]
        [Display(Name = "Poststed")]
        public string? City { get; set; }
        [Required]
        [Display(Name = "E-post")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }
    }
}
