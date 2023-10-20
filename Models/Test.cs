// Relocated use of this class from deleted DB context to ServiceOrderDatabase.cs

using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
    }
}
