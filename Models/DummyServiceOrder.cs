using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public class DummyServiceOrder
    {
        [Key] // This annotation specifies the primary key
        public int ServiceOrderID { get; set; }

        [DisplayName("Ordrenummer")]
        public int OrderNum { get; set; }
        [DisplayName("Ordredato")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Fornavn")]
        public string? CustomerFirstname { get; set; }
        [DisplayName("Etternavn")]
        public string? CustomerLastname { get; set; }
    }
}

